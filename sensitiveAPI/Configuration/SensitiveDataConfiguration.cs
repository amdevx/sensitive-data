
using Microsoft.Extensions.Configuration;

namespace sensitiveAPI.Configuration
{
    public class SensitiveDataConfiguration : ConfigBase, IDatabaseConfig
    {
        public SensitiveDataConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        public string DatabaseConnectionString => Get("DATABASE_CONNECTIONSTRING",
            "Data Source=(local);Initial Catalog=SensitiveData;Integrated Security=true");
    }
}