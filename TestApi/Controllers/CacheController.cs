using Infrastructure;
using Infrastructure.Database.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private readonly ILogger<CacheController> _logger;
    private readonly ICacheService _cacheService;

    public CacheController(ILogger<CacheController> logger, IConfiguration config, ICacheService cacheService)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] Dictionary<string, string> data)
    {
        foreach (string key in data.Keys)
        {
            await _cacheService.Set(key, data[key], TimeSpan.FromMinutes(5));
        }
        return Ok();
    }

    [HttpGet("Retrieve")]
    public async Task<IActionResult> Retrieve(string key)
    {
        var value = await _cacheService.Get<string>(key);
        return Ok(value);
    }
}
