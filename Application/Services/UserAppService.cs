using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserAppService
    {
        private readonly ILogger<UserAppService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly ICacheKeyGenerator _cacheKeyGenerator;
        private readonly UserDomainService _userDomainService;
        private readonly IPasswordService _passwordService;

        public UserAppService(ILogger<UserAppService> logger, IUnitOfWork unitOfWork, ICacheService cacheService, ICacheKeyGenerator cacheKeyGenerator, UserDomainService userDomainService, IPasswordService passwordService)
        {
            _logger = logger;
            _userDomainService = userDomainService;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _cacheKeyGenerator = cacheKeyGenerator;
            _cacheService = cacheService;
        }

        public async Task<UserEntity?> RetrieveUserById(Guid id)
        {
            UserEntity? user;
            user = await _cacheService.GetOrCreate<UserEntity>(
                _cacheKeyGenerator.ForEntity<UserEntity>(id.ToString()), 
                () => _unitOfWork.UserRepository.FindById(id)
            );
            return user ?? default;
        }

        public async Task<UserEntity?> CreateUser(string username, string password, string email, string role)
        {
            UserEntity user = _userDomainService.CreateUser(username, password, email, role, _passwordService);

            _unitOfWork.UserRepository.Add(user);
            await _cacheService.Set(_cacheKeyGenerator.ForEntity<UserEntity>(user.Id.ToString()), user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }
    }
}
