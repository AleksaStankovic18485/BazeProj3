using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class RadnoTeloController : ControllerBase
    {
        [HttpGet("PruzmiSvaRadnaTela")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiSvaRadnaTela()
        {
            var (isError, odeljenja, error) = await DTOManager.VratiSvaRadnaTela();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(odeljenja);
        }
        [HttpGet("VratiRadnoTelo/{gg}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiRadnoTelo(string gg)
        {
            var result = await DTOManager.VratiRadnoTelo(gg);

            if (result.IsError)
            {
                return StatusCode(result.Error?.StatusCode ?? 400, result.Error?.Message);
            }

            return Ok(result.Data);
        }


    }
}
