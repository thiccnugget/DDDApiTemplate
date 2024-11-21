using Infrastructure.Database.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(IUnitOfWork unitOfWork, ILogger<DatabaseController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("Save")]
        public IActionResult Save([FromBody]UserEntity user)
        {
            _unitOfWork.BeginTransaction();
            _unitOfWork.UserRepository.Save(user);
            _unitOfWork.CommitTransaction();
            return Ok(user);
        }

        [HttpGet("Retrieve/{username}")]
        public IActionResult Get(string username)
        {
            var user = _unitOfWork.UserRepository.FindByUsername(username);
            return user is null ? NotFound() : Ok(user);
        }
    }
}
