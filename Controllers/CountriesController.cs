using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_AuthAndRefrest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var listCountries = await Task.FromResult(new List<string> { "Ecuador", "Argentina", "España", "Croacia", "Brazil", "Alemania"});
            return Ok(listCountries);
        }
    }
}
