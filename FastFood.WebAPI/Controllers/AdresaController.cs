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
    public class AdresaController : ControllerBase
    {
        protected IAdresa _service;

        public AdresaController(IAdresa service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("adresa/korisnik/{korisnikId}")]
        public IEnumerable<AdresaDto> GetAdreseByKorisnikId(string korisnikId)
        {
            return _service.GetAdreseByKorisnikId(korisnikId);
        }

        [HttpGet]
        [Route("{adresaId}")]
        public ActionResult<AdresaDto> GetAdresaById(int adresaId)
        {
            var adresa = _service.GetAdreseById(adresaId);
            return adresa;
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAdresaAsync([FromBody] AdresaREST adresaREST)
        {
            try
            {
                AdresaDto adresaDto = new AdresaDto();
                adresaDto.KorisnikId = adresaREST.KorisnikId;
                adresaDto.Grad = adresaREST.Grad;
                adresaDto.Ulica = adresaREST.Ulica;
                adresaDto.BrojStana = adresaREST.BrojStana;
                adresaDto.Kat= adresaREST.Kat;
                bool add_adresa = await _service.AddAdresaAsync(adresaDto);
                if (add_adresa)
                {
                    return Ok("Adresa dodana");
                }
                else
                {
                    return Ok("Adresa nije dodana");
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Greška pri dodavanju adrese: " + ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> UpdateAdresa([FromBody] AdresaUpdateREST adresaREST)
        {
            try
            {
                AdresaDto adresaDto = new AdresaDto();
                adresaDto.Id = adresaREST.Id;
                adresaDto.Grad = adresaREST.Grad;
                adresaDto.Ulica = adresaREST.Ulica;
                adresaDto.BrojStana = adresaREST.BrojStana;
                adresaDto.Kat = adresaREST.Kat;
                bool edit_adresa = await _service.UpdateAdresaAsync(adresaDto);
                if (edit_adresa)
                {
                    return Ok("Adresa dodana");
                }
                else
                {
                    return Ok("Adresa nije dodana");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri spremanju adrese: " + ex.Message, ex);
            }
        }
        [HttpDelete]
        [Route("delete/{adresaId}")]
        public async Task<IActionResult> DeleteAdresa(int adresaId)
        {
            try
            {
                bool obrisana = await _service.DeleteAdresaAsync(adresaId);
                if (obrisana)
                    return Ok("Adresa obrisana");
                else
                    return NotFound("Adresa nije pronađena");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Greška pri brisanju adrese: " + ex.Message);
            }
        }


    }


}
