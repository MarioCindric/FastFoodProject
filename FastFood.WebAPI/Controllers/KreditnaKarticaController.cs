using FastFood.Model;
using FastFood.Service;
using FastFood.Service.Common;
using FastFood.WebAPI.RESTModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.WebAPI.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class KreditnaKarticaController : ControllerBase
    {
        protected IKreditnaKartica _service;

        public KreditnaKarticaController(IKreditnaKartica service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("kartica/korisnik/{korisnikId}")]
        public IEnumerable<KreditnaKarticaDto> GetKarticeByKorisnikId(string korisnikId)
        {
            return _service.GetKarticeByKorisnikId(korisnikId);
        }

        [HttpPost]
        [Route("add")]

        public async Task<IActionResult> AddKarticaAsync([FromBody] KarticaREST karticaREST)
        {
            try
            {
                KreditnaKarticaDto karticaDto = new KreditnaKarticaDto();
                karticaDto.KorisnikId = karticaREST.KorisnikId;
                karticaDto.BrojKartice = karticaREST.BrojKartice;
                karticaDto.DatumIsteka = karticaREST.DatumIsteka;
                karticaDto.SigurnosniKod = karticaREST.SigurnosniKod;
                bool add_kartica = await _service.AddKarticaAsync(karticaDto);
                if (add_kartica)
                {
                    return Ok("Kartica dodana");
                }
                else
                {
                    return Ok("Kartica nije dodana");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri dodavanju kartice: " + ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("edit")]

        public async Task<IActionResult> UpdateKarticaAsync([FromBody] KarticaUpdateREST karticaREST)
        {
            try
            {
                KreditnaKarticaDto karticaDto = new KreditnaKarticaDto();
                karticaDto.Id = karticaREST.Id;
                karticaDto.BrojKartice = karticaREST.BrojKartice;
                karticaDto.DatumIsteka = karticaREST.DatumIsteka;
                karticaDto.SigurnosniKod = karticaREST.SigurnosniKod;
                bool update_kartica = await _service.UpdateKarticaAsync(karticaDto);
                if (update_kartica)
                {
                    return Ok("Kartica azurirana");
                }
                else
                {
                    return Ok("Kartica nije azurirana");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri azuriranju kartice: " + ex.Message, ex);
            }
        }

        [HttpDelete]
        [Route("delete/{karticaId}")]
        public async Task<IActionResult> DeleteKarticaAsync(int karticaId)
        {
            try
            {
                bool obrisana = await _service.DeleteKarticaAsync(karticaId);
                if (obrisana)
                    return Ok("Kartica obrisana");
                else
                    return NotFound("Kartica nije pronađena");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Greška pri brisanju kartice: " + ex.Message);
            }
        }

    }

}
