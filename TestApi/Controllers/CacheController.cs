using Application.Interfaces;
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
        
        await Task.WhenAll(data.Keys.Select(async key => await _cacheService.SetAsync(key, data[key])));
        return Ok();
    }

    [HttpGet("Retrieve")]
    public async Task<IActionResult> Retrieve(string key)
    {
        var value = await _cacheService.GetAsync<string>(key);
        return Ok(value);
    }
}
