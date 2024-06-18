using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RadniDanController : ControllerBase
    {
        [HttpDelete]
        [Route("IzbrisiRadniDan/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteRadniDan(int id)
        {
            var data = await DTOManager.ObrisiRadniDan(id);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(204, $"Uspešno obrisana prodavnica. ID: {id}");
        }

        [HttpGet("VratiRadniDan/{IDRD}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiRD(int IDRD)
        {
            var result = await DTOManager.VratiRadniDanAsync(IDRD);

            if (result.IsError)
            {
                return StatusCode(result.Error?.StatusCode ?? 400, result.Error?.Message);
            }

            return Ok(result.Data);
        }

        [HttpPut]
        [Route("PromeniRadniDan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeRD([FromBody] RadniDanView p)
        {
            (bool isError, var rd, ErrorMessage? error) = await DTOManager.AzurirajRadniDanAsync(p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (rd == null)
            {
                return BadRequest("RadniDan ID nije validan.");
            }

            return Ok($"Uspešno ažuriran RadniDan. DatumOd: {rd.DatumP} DatumDo:{rd.DatumEnd}");
        }

        [HttpPost]
        [Route("DodajRadniDan")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddRD([FromBody] RadniDanView p)
        {
            var data = await DTOManager.DodajRadniDanAsync(p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat RadniDan.DatumOd: {p.DatumP} DatumDo:{p.DatumEnd}");
        }

        [HttpGet]
        [Route("PreuzmiRDSednice/{sedID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PreuzmiRDSednice(int sedID)
        {
            (bool isError, var radnici, var error) = await DTOManager.VratiSveRDaneSednice(sedID);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(radnici);
        }
        [HttpGet]
        [Route("PreuzmiRD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRD()
        {
            var radnici = await DTOManager.VratiSveRadneDaneAsync();

            if (radnici.IsError)
            {
                return StatusCode(radnici.Error.StatusCode, radnici.Error.Message);
            }

            return Ok(radnici.Data);
        }
    }
}
