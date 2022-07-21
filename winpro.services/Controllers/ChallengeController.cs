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
        public ChallengeController(ICoin coinRepository)
        {
            this._coinRepository = coinRepository;
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
            return Ok(await _coinRepository.GetItemFila());
        }

    }
}