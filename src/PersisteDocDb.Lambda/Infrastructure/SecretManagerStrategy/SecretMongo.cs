using Microsoft.Extensions.Configuration;

namespace PersisteDocDb.Lambda.Infrastructure.SecretManagerStrategy
{
    public class SecretMongo : ITypeSecret
    {
        private readonly string _secretName;
        public SecretMongo(string secretName)
        {
            _secretName = secretName;
        }

        private string GetConnectionString(ref dynamic secret)
        {
            return $"mongodb://{secret.username}:{secret.password}@{secret.host}:{secret.port}/?retryWrites=false";
        }

        public string GetSecretName()
        {
            return _secretName;
        }

        public void SetConfiguration(ref IConfiguration configuration, ref dynamic secret)
        {
            configuration[$"Database:{_secretName}CONNECTIONSTRING"] = GetConnectionString(ref secret);
        }
    }
}
