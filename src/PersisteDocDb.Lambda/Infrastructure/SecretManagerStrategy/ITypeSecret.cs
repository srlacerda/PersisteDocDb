using Microsoft.Extensions.Configuration;

namespace PersisteDocDb.Lambda.Infrastructure.SecretManagerStrategy
{
    public interface ITypeSecret
    {
        //public void SetConfiguration(ref IConfiguration configuration, ref dynamic secret);
        public void SetConfiguration(ref IConfiguration configuration, dynamic secret);

        public string GetSecretName();
    }
}
