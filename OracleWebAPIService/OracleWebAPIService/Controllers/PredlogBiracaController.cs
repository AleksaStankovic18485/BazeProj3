using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredlogBiracaController : ControllerBase
    {
        [HttpDelete]
        [Route("IzbrisiPAKT/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeletePAKT(int id)
        {
            var data = await DTOManager.ObrisiPAktAsync(id);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(204, $"Uspešno obrisan PAKT: {data.Data}.");
        }

        [HttpGet("VratiPBiraca/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiPBAsync(int ID)
        {
            var result = await DTOManager.VratiPBAsync(ID);

            if (result.IsError)
            {
                return StatusCode(result.Error?.StatusCode ?? 400, result.Error?.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("VratiPA/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiPAAsync()
        {
            var result = await DTOManager.VratiPAktAsync();

            if (result.IsError)
            {
                return StatusCode(result.Error?.StatusCode ?? 400, result.Error?.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("SacuvajPredlogBiraca")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddPredlogBiraca([FromBody] PredlozioBiraciView s)
        {
            (bool isError, int id, var error) = await DTOManager.SacuvajPredlogBiraca(s);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return StatusCode(201, $"Sačuvan je predlog Biraca sa ID: {id}");
        }

        [HttpPost]
        [Route("SacuvajPredlogPoslanika")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddSlagalica([FromBody] PredlozioNarodniPoslanikView s)
        {
            (bool isError, int id, var error) = await DTOManager.SacuvajPredlogPoslanika(s);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return StatusCode(201, $"Sačuvan je predlog Poslanika sa ID: {id}");
        }

        [HttpDelete]
        [Route("IzbrisiPAkt/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteSlagalica(int id)
        {
            var data = await DTOManager.ObrisiPAktAsync(id);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(204, $"Uspešno izbrisan PAkt sa ID: {id}");
        }
    }
}
