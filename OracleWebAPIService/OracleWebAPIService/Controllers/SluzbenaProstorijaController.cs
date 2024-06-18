using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OracleWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SluzbenaProstorijaController : ControllerBase
    {


        [HttpGet]
        [Route("ProstorijePGrupe/{PGrupaID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ProstPGrupe(int PGrupaID)
        {
            (bool isError, var radnici, var error) = DTOManager.VratiPGrupaProstorije(PGrupaID);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(radnici);
        }

        [HttpGet("VratiSlzubenuProstoriju/{IDSluz}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiSluzbP(int IDSluz)
        {
            var result = await DTOManager.VratiSluzbenaProstorijaAsync(IDSluz);

            if (result.IsError)
            {
                return StatusCode(result.Error?.StatusCode ?? 400, result.Error?.Message);
            }

            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("IzbrisiSluzbenuProstoriju/{idSluzbene}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteSluzbP(int idSluzbene)
        {
            var data = await DTOManager.ObrisiSluzbenaProstorija(idSluzbene);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(204, $"Uspešno obrisana sluzbena prostorija: {data.Data}.");
        }

        [HttpGet]
        [Route("VratiSveSluzbeneProstorije")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetSluzbP()
        {
            var prost = await DTOManager.VratiSveSluzbeneProstorije();

            if (prost.IsError)
            {
                return StatusCode(prost.Error.StatusCode, prost.Error.Message);
            }

            return Ok(prost.Data);
        }

        [HttpPut]
        [Route("AzurirajSluzbenuProst")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeSluzbenuP([FromBody] SluzbenaProstorijaView p)
        {
            (bool isError, var sluzp, ErrorMessage? error) = await DTOManager.AzurirajSluzbenuProstorijuAsync(p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (sluzp == null)
            {
                return BadRequest("Prodavnica nije validna.");
            }
            return Ok($"Uspešno ažurirana SluzbP. Broj: {sluzp.BrojProstorije}");

        }

        [HttpPost]
        [Route("DodajSluzbenuP")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddSluzbP([FromBody] SluzbenaProstorijaView p)
        {
            var data = await DTOManager.DodajSluzbenuProstoriju(p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata prodavnica. Broj: {p.BrojProstorije}");
        }
    }
}