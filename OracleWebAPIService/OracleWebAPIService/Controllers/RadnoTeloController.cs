using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class RadnoTeloController : ControllerBase
    {
        /*[HttpGet("PruzmiSvaRadnaTela")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiSvaRadnaTela()
        {
            var (isError, rT, error) = await DTOManager.vratr();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(odeljenja);
        }*/
    }
}
