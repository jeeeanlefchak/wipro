using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using winpro.business.Data;
using winpro.business.Implementation;
using winpro.services.Model;

namespace winpro.services.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class ChallengeController : ControllerBase
    {
        private readonly ICoin _coinRepository;
        private readonly IServiceScopeFactory _scopeFactory;
        public ChallengeController(ICoin coinRepository, IServiceScopeFactory scopeFactory)
        {
            this._coinRepository = coinRepository;
            _scopeFactory = scopeFactory;   
        }

        [HttpPost("AddItemFila")]
        public async Task<IActionResult> AddItemFila([FromBody] Coin coin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _coinRepository.AddItemFila(coin));
        }

        [HttpGet("GetItemFila")]
        public async Task<IActionResult> GetItemFila()
        {
            return Ok( _coinRepository.GetItemFila(this._scopeFactory));
        }

    }
}