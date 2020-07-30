
using Microsoft.Extensions.Configuration;

namespace sensitiveAPI.Configuration
{
    public class SensitiveDataConfiguration : ConfigBase, IDatabaseConfig, IEncryptionKeysConfig
    {
        public SensitiveDataConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        public string DatabaseConnectionString => Get("DATABASE_CONNECTIONSTRING",
            "Data Source=(local);Initial Catalog=SensitiveData;Integrated Security=true");

        string IEncryptionKeysConfig.CurrentKeyName => Get("CURRENTKEYNAME", string.Empty);

        string IEncryptionKeysConfig.GetKeyByName(string keyName)
        {
            return Get($"{keyName.ToUpperInvariant()}", string.Empty);
        }

        string IEncryptionKeysConfig.GetCurrentKey()
        {
            return Get($"{((IEncryptionKeysConfig) this).CurrentKeyName.ToUpperInvariant()}",
                string.Empty);
        }
    }
}