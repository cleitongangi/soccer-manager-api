using Microsoft.EntityFrameworkCore;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Infra.Data.Context;

namespace SoccerManager.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISoccerManagerDbContext _db;

        public UserRepository(ISoccerManagerDbContext soccerManagerDbContext)
        {
            _db = soccerManagerDbContext;
        }

        public async Task<bool> HasAsync(string userName)
        {
            return await _db.Users
                .AnyAsync(x => x.Username.Equals(userName));
        }

        public async Task AddAsync(UserEntity entity)
        {
            await _db.Users.AddAsync(entity);
        }

        public async Task<UserEntity?> LoginAsync(string username)
        {
            return await _db.Users
                .Where(x => x.Username.Equals(username))
                .Select(x => new UserEntity(x.Id, x.Username, x.Password))
                .FirstOrDefaultAsync();
        }
    }
}
