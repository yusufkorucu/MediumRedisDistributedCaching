using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisDistributedCaching.Api.Const;

namespace RedisDistributedCaching.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public CacheController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("Set")]
        public async Task<IActionResult> Set(string nameSurname)
        {
            await _distributedCache.SetStringAsync(RedisCacheKey.NameSurnameKey, nameSurname,
                options: new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1),
                    SlidingExpiration = TimeSpan.FromSeconds(15)
                });

            return Ok();
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var nameSurname = await _distributedCache.GetStringAsync(RedisCacheKey.NameSurnameKey);

            return Ok(nameSurname);
        }
    }
}
