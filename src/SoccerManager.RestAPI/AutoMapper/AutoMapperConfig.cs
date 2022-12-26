using AutoMapper;
using SoccerManager.RestAPI.AutoMapper.Profiles;

namespace SoccerManager.RestAPI.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SoccerManagerMappingProfile());
            })
            .AssertConfigurationIsValid();
        }
    }
}
