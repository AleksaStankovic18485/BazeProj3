using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StalniRadniciController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiStalneRadnike")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetStalneRadke()
        {
            (bool isError, var prodavnice, ErrorMessage? error) = DTOManager.VratiSveStalneRadnike();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(prodavnice);
        }

        [HttpGet]
        [Route("PreuzmiStalnogRadnika/{BrojRadneKnjizice}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetStalnogRadnika(int BrojRadneKnjizice)
        {
            (bool isError, var prodavnice, ErrorMessage? error) = DTOManager.VratiStalnog(BrojRadneKnjizice);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(prodavnice);
        }
        [HttpDelete]
        [Route("IzbrisiStalnog/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteStalnog(int id)
        {
            var data = await DTOManager.ObrisiStalnog(id);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(204, $"Uspešno obrisan Stalni Radnik. ID: {id}");
        }
        [HttpGet]
        [Route("PreuzmiStalneOdnosRadnike/{Identifikacioni}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PreuzmiSefoveProdavnice(int Identifikacioni)
        {
            (bool isError, var radnici, var error) = await DTOManager.VratiSveStalneOdnosRadnike(Identifikacioni);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }
            return Ok(radnici);

        }
    }
}
