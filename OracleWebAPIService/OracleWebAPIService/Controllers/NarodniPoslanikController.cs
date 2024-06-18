using Microsoft.AspNetCore.Mvc;
using DataBaseAccess;
using DataBaseAccess.DTO_s;

namespace OracleWebAPIService.Controllers;

[ApiController]
[Route("[controller]")]
public class NarodniPoslanikController : ControllerBase
{
    [HttpGet]
    [Route("PreuzmiSveNarodnePoslanike")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetRadnici()
    {
        var np = await DTOManager.VratiSveNarodnePoslanikeAsync();

        if (np.IsError)
        {
            return StatusCode(np.Error.StatusCode, np.Error.Message);
        }

        return Ok(np.Data);
    }
/*    [HttpGet]
    [Route("PreuzmiStalneRadnike/{prodavnicaID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PreuzmiSefoveProdavnice(int prodavnicaID)
    {
        (bool isError, var radnici, var error) = await DTOManager.VratiStalnog(prodavnicaID);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(radnici);
    }*/

    [HttpGet]
    [Route("PreuzmiSveNarodnePoslanikePGrupe/{prodavnicaNaziv}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetNarodnePoslanikePGrupe(string prodavnicaNaziv)
    {
        (bool isError, var np, var error) = DTOManager.VratiSveNarodnePoslanikePgrupe(prodavnicaNaziv);

        if (isError)
        {
            return StatusCode(error?.StatusCode ?? 400, error?.Message);
        }

        return Ok(np);
    }

    [HttpPost]
    [Route("DodajNarodnogPoslanika")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddNarodniPoslanik([FromBody] NarodniPoslanikView r)
    {
        var data = await DTOManager.DodajPoslanikaAsync(r);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(201, $"Uspešno dodat NP: {r.Ime}");
    }

    [HttpPost]
    [Route("PoveziPrisutnost/{npID}/{sednicaID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LinkRadnik(int npID, int sednicaID)
    {
        (bool isError1, var np, var error1) = await DTOManager.VratiNarodnogAsync(npID);
        (bool isError2, var sed, var error2) = await DTOManager.VratiSednicuAsync(sednicaID);

        if (isError1 || isError2)
        {
            return StatusCode(error1?.StatusCode ?? 400, $"{error1?.Message}{Environment.NewLine}{error2?.Message}");
        }

        if (np == null || sed == null)
        {
            return BadRequest("NarodniPoslanik ili Sednica nisu validni.");
        }
        //ovde treba ide dodaj
        var data = await DTOManager.DodajPrisutnostAsync(new BioPrisutanView
        {
            // Datum bi trebalo da se šalje putem parametra
            DatumOd = DateTime.Now.AddYears(-1),
            DatumDo = DateTime.Now.AddDays(1),

            Id = new BioPrisutanIdView
            {
                NPPrisutan = np,
                SednicaZasedanja = sed
            }
        });

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Dodat radni odnos. Radnik: {np.Ime} {np.Prezime}. Prodavnica: {sed.DatumStart}");
    }

    /*[HttpPost]
    [Route("PoveziStanjeNP-a/{pg}/{radnoT}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
   *//* public async Task<IActionResult> LinkStanjeNP(string pg, string radnoT)
    {
        (bool isError1, var pgrupa, var error1) = await DTOManager.VratiPGrupuAsync(pg);
        (bool isError2, var radno, var error2) = await DTOManager.VratiRadnoTelo(radnoT);

        if (isError1 || isError2)
        {
            return StatusCode(error1?.StatusCode ?? 400, $"{error1?.Message}{Environment.NewLine}{error2?.Message}");
        }

        if (pgrupa == null || radno == null)
        {
            return BadRequest("PGrupa ili RadnoT nisu validni.");
        }
        //ovde treba ide dodaj
        var data = await DTOManager.VratiNPAnketni(new NarodniPoslanikView
        {
 
                PGrupa = pgrupa,
                RadnoT = radno
         
        }) ;

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return Ok($"Dodat radni odnos. Pgrupa: {pgrupa.Naziv}. Tip: {radno.Tip}");
    }*/

    [HttpDelete]
    [Route("IzbrisiNarodnogPoslanika/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteRadnik(int id)
    {
        var data = await DTOManager.ObrisiNarodnogAsync(id);

        if (data.IsError)
        {
            return StatusCode(data.Error.StatusCode, data.Error.Message);
        }

        return StatusCode(204, $"Uspešno obrisan NarodniPoslanik: {data.Data}.");
    }

}