using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateJwt(Guid userid)
        {
            UserEntity? user = await _unitOfWork.UserRepository.FindByIdAsync(userid);

            if (user is null)
            {
                return NotFound();
            }

            string token = _authService.GenerateJwt(user);
            return Ok(token);

        }
    }
}
