using Microsoft.Extensions.Configuration;

namespace PersisteDocDb.Lambda.Infrastructure.SecretManagerStrategy
{
    public class SecretMySql : ITypeSecret
    {
        private readonly string _secretName;
        public SecretMySql(string secretName)
        {
            _secretName = secretName;
        }

        private string GetConnectionString(ref dynamic secret)
        {
            return $"Server={secret.dbIntanceIdentifier}; " +
                         $"Database{secret.dbname};" +
                         $"Uid={secret.username};" +
                         $"Pwd={secret.password}";
        }

        public string GetSecretName()
        {
            return _secretName;
        }

        public void SetConfiguration(ref IConfiguration configuration, dynamic secret)
        {
            configuration[$"Database:{_secretName}CONNECTIONSTRING"] = GetConnectionString(ref secret);
        }
    }
}
