using SoccerManager.Domain.Interfaces.GlobalSettings;

namespace SoccerManager.RestAPI.GlobalSettings
{
    public class ConfigSettings : IConfigSettings
    {        
        public int DefaultPaginationPageSize { get; private set; }
        
        public ConfigSettings(IConfiguration configuration)
        {
            DefaultPaginationPageSize = configuration.GetSection("AppSettings").GetValue<int>("DefaultPaginationPageSize");
        }
    }
}
