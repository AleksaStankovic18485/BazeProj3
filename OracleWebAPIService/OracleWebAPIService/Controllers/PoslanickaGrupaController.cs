using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoslanickaGrupaController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiPGrupe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetProdavnice()
        {     
            (bool isError, var pgrupe, ErrorMessage? error) = DTOManager.VratiSvePGrupa();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(pgrupe);
        }

        [HttpPost]
        [Route("DodajPGrupu")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddProdavnica([FromBody] PoslanickaGrupaView p)
        {
            var data = await DTOManager.DodajPoslanickuGrupuAsync(p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata Pgrupa. Naziv: {p.Naziv}");
        }

        [HttpPut]
        [Route("PromeniPGrupu")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeProdavnica([FromBody] PoslanickaGrupaView p)
        {
            (bool isError, var pgrupa, ErrorMessage? error) = await DTOManager.AzurirajPGrupuAsync(p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (pgrupa == null)
            {
                return BadRequest("PGrupa nije validna.");
            }

            return Ok($"Uspešno ažurirana PGrupa. Naziv: {pgrupa.Naziv}");
        }

        [HttpDelete]
        [Route("IzbrisiPGrupu/{naz}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteProdavnica(string naz)
        {
            var data = await DTOManager.ObrisiPgrupuAsync(naz);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(204, $"Uspešno obrisana PGrupa. Naziv(ID): {naz}");
        }
    }
}
