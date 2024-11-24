using Application.Interfaces;
using Domain.Entities;
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
        public async Task<IActionResult> Save([FromBody]UserEntity user)
        {
            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveAsTransaction();
            return Ok(user.Id);
        }

        [HttpGet("Retrieve")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<UserEntity> user = await _unitOfWork.UserRepository.Find();
            return Ok(user);
        }

        [HttpGet("Retrieve/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserEntity? user = await _unitOfWork.UserRepository.FindById(id);
            return user is null ? NotFound() : Ok(user);
        }
    }
}
