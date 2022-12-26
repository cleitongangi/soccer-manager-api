using FluentValidation.Results;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserEntity?> GetByUsernameAndPasswordAsync(string username, string password);
        Task<ValidationResult> SignUpAsync(string username, string password);
    }
}
