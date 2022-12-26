using FluentValidation.Results;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Data;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Services;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserAddValidation _userAddValidation;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _hashService;
        private readonly ITeamService _teamService;

        public UserService(IUnitOfWork unitOfWork,
                           IUserAddValidation userAddValidation,
                           IUserRepository userRepository,
                           IPasswordHashService hashService,
                           ITeamService teamService)
        {
            this._uow = unitOfWork;
            this._userAddValidation = userAddValidation;
            this._userRepository = userRepository;
            this._hashService = hashService;
            this._teamService = teamService;
        }

        #region Public Methods
        public async Task<ValidationResult> SignUpAsync(string username, string password)
        {
            // Create new User
            var entity = new UserEntity(username, _hashService.HashPassword(password), DateTime.Now);
            try
            {
                await _uow.BeginTransactionAsync();

                var result = await AddAsync(entity);
                if (!result.IsValid)
                {
                    await _uow.RollbackAsync();
                    return result;
                }
                
                // Create initial Teams and players
                await _teamService.CreateInitialTeamAndPlayersAsync(entity.Id); // TeamId and userId is equal because is one user to one team.

                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
                return result;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<UserEntity?> GetByUsernameAndPasswordAsync(string username, string password)
        {            
            var user = await _userRepository.LoginAsync(username);
            if(user == null)
            {
                return null;
            }

            // Check password hash
            if (_hashService.VerifyPassword(password, user.Password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        #endregion Public Methods

        #region Private Methods
        private async Task<ValidationResult> AddAsync(UserEntity entity)
        {
            var result = await _userAddValidation.ValidateAsync(entity);
            if (!result.IsValid)
            {
                return result;
            }

            await _userRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return result;
        }
        #endregion Private Methods
    }
}
