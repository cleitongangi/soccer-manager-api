using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Domain.Interfaces.Services;

namespace SoccerManager.Infra.Security
{
    public static class RegisterIoC
    {
        public static void Register(IServiceCollection services)
        {
            #region Registra todos os Validations
            services.AddSingleton<IPasswordHashService, PasswordHashService>();
            #endregion Registra todos os Validations
        }
    }
}
