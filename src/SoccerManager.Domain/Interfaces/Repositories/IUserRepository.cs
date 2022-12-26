using SoccerManager.Domain.Entities;

namespace SoccerManager.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(UserEntity entity);
        Task<bool> HasAsync(string userName);
        Task<UserEntity?> LoginAsync(string username);
    }
}
