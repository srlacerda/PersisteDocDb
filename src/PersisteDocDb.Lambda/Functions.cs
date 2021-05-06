using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersisteDocDb.Lambda.Application.Extensions;
using PersisteDocDb.Lambda.Infrastructure.Logging;
using PersisteDocDb.Lambda.Infrastructure.Repositories;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PersisteDocDb.Lambda
{
    public class Functions
    {
        protected IServiceProvider _serviceProvider = null;
        protected ServiceCollection _serviceCollection = new ServiceCollection();
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Functions()
        {
            ConfigureServices();
        }

        public Functions(ServiceCollection serviceCollection)
        {
            IConfigurationRoot configuration = GetConfiguration();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices()
        {
            IConfigurationRoot configuration = GetConfiguration();

            _serviceCollection.AddSingleton<IConfiguration>(configuration);

            //Repositories
            _serviceCollection.AddTransient<IPosicaoDocumentRepository, PosicaoDocumentRepository>();

            _serviceCollection.AddMediatorHandlers(typeof(Functions).Assembly);
            _serviceCollection.AddSingleton<ILogger, Logger>();
            _serviceCollection.AddScoped<IMediator, Mediator>();
            _serviceCollection.AddScoped<ServiceFactory>(sp => sp.GetService);

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables();

            var configuration = builder.Build();

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
