using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SednicaController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiSednice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetSednice()
        {
            (bool isError, var sed, ErrorMessage? error) = DTOManager.VratiSveSednice();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sed);
        }
        [HttpDelete]
        [Route("IzbrisiSednice/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteSednica(int id)
        {
            var data = await DTOManager.ObrisiSednicuAsync(id);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(204, $"Uspešno obrisana sednica. ID: {id}");
        }

        [HttpPost]
        [Route("DodajSednicu")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddSednica([FromBody] SednicaView p)
        {
            var data = await DTOManager.DodajSednicuAsync(p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata Sednica. ID: {p.Id}");
        }

        [HttpPut]
        [Route("PromeniSednicu")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeSednica([FromBody] SednicaView p)
        {
            (bool isError, var sed, ErrorMessage? error) = await DTOManager.AzurirajSednicuAsync(p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (sed == null)
            {
                return BadRequest("Sednica nije validna.");
            }

            return Ok($"Uspešno ažurirana sednica. Naziv: {sed.Id}");
        }

        [HttpGet("VratiSednicu/{IDSed}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiSluzbP(int IDSed)
        {
            var result = await DTOManager.VratiSednicuAsync(IDSed);

            if (result.IsError)
            {
                return StatusCode(result.Error?.StatusCode ?? 400, result.Error?.Message);
            }

            return Ok(result.Data);
        }
    }
}
