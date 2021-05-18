using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SecretsManager;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pacote.Core.Domain.Model.Enums;
using Pacote.Core.Domain.Util.DocumentDB;
using Pacote.Infrastructure.Data.DocumentDB;
using PersisteDocDb.Lambda.Application.Extensions;
using PersisteDocDb.Lambda.Domain.Entities;
using PersisteDocDb.Lambda.Infrastructure.Factory;
using PersisteDocDb.Lambda.Infrastructure.Logging;
using PersisteDocDb.Lambda.Infrastructure.Repositories;
using PersisteDocDb.Lambda.Infrastructure.SecretManagerStrategy;
using System;
using System.IO;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PersisteDocDb.Lambda
{
    public class Functions
    {
        protected IServiceProvider _serviceProvider = null;
        protected ServiceCollection _serviceCollection = new ServiceCollection();
        protected string _database = DatabaseEnum.FidhDocdbTeste.ToString().ToUpper();
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Functions()
        {
            ConfigureServices(GetConfiguration());
        }

        public Functions(ServiceCollection serviceCollection)
        {
            IConfigurationRoot configuration = GetConfiguration();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IConfigurationRoot configurationRoot)
        {
            //IConfigurationRoot configuration = GetConfiguration();
            IConfigurationRoot configuration = configurationRoot;

            _serviceCollection.AddSingleton<IConfiguration>(configuration);

            //Factory
            _serviceCollection.AddTransient<IDocumentFactory<PosicaoDocument>, PosicaoFactory>();
            _serviceCollection.AddTransient<IDocumentFactory<OperacaoDocument>, OperacaoFactory>();

            //Repositories
            _serviceCollection.AddTransient<IDocumentRepository<PosicaoDocument>, PosicaoDocumentRepository>();
            _serviceCollection.AddTransient<IDocumentRepository<OperacaoDocument>, OperacaoDocumentRepository>();

            //Infra
            MongoClient mongoClient = new MongoClient(configuration[$"Database:{_database}CONNECTIONSTRING"]);
            string defaultDatabase = configuration[$"Database:{_database}DEFAULTDATABASE"];

            _serviceCollection.AddScoped<IDocumentCollection<PosicaoDocument>>(sp => InstanceDocumentCollection<PosicaoDocument>(mongoClient, defaultDatabase, "posicao"));
            _serviceCollection.AddScoped<IDocumentCollection<OperacaoDocument>>(sp => InstanceDocumentCollection<OperacaoDocument>(mongoClient, defaultDatabase, "operacao"));

            _serviceCollection.AddMediatorHandlers(typeof(Functions).Assembly);
            _serviceCollection.AddSingleton<ILogger, Logger>();
            _serviceCollection.AddScoped<IMediator, Mediator>();
            _serviceCollection.AddScoped<ServiceFactory>(sp => sp.GetService);

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        private IDocumentCollection<T> InstanceDocumentCollection<T>(MongoClient mongoClient, string databaseId, string collectionId) where T : IDocument
        {
            return new DocumentCollection<T>(mongoClient, databaseId, collectionId);
        }

        private static IConfigurationRoot GetConfiguration()
        {
//#if DEBUG
//            var env = "dev";
//#else
//            var env = Environment.GetEnvironmentVariable("env");
//#endif

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                //.AddJsonFile($"appsettings.{env}.json")
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            
            configuration.AddDatabaseConnectionString(
                new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(configuration["AWS_REGION"])),
                new SecretMongo(DatabaseEnum.FidhDocdbTeste.ToString().ToUpper()));

            return configuration;
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed message {message.Body}");

            // TODO: Do interesting work based on the new message
            await Task.CompletedTask;
        }
    }
}
