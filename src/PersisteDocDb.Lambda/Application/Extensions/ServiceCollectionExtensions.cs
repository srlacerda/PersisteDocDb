using Amazon;
using Amazon.Lambda.Core;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PersisteDocDb.Lambda.Infrastructure.SecretManagerStrategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PersisteDocDb.Lambda.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services, Assembly assembly)
        {

            var classTypes = assembly.ExportedTypes.Select(t => t.GetTypeInfo()).Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in classTypes)
            {
                var interfaces = type.ImplementedInterfaces.Select(i => i.GetTypeInfo());

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)))
                {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }
            }
            return services;

        }

        //public static void AddDatabaseConnectionString(this IConfiguration configuration, ITypeSecret typeSecret)
        public static void AddDatabaseConnectionString(this IConfiguration configuration, IAmazonSecretsManager amazonSecretsManager, ITypeSecret typeSecret)
        {
            dynamic secret;

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = typeSecret.GetSecretName(),
                VersionStage = "AWSCURRENT"
            };

            GetSecretValueResponse response = amazonSecretsManager.GetSecretValueAsync(request).Result;

            if (response.SecretString != null)
            {
                secret = JsonConvert.DeserializeObject<dynamic>(response.SecretString);
            }
            else
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                secret = JsonConvert.DeserializeObject<dynamic>
                    (System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd())));
            }

            //TODO APAGAR DEPOIS
            string engine = secret.engine;
            LambdaLogger.Log(engine);

            typeSecret.SetConfiguration(ref configuration, secret);

            //TODO APAGAR DEPOIS
            LambdaLogger.Log(configuration[$"Database:{typeSecret.GetSecretName()}CONNECTIONSTRING"]);
        }
    }
}
