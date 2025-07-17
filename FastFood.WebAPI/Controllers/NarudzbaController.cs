using FastFood.Model;
using FastFood.Service;
using FastFood.Service.Common;
using FastFood.WebAPI.RESTModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NarudzbaController : ControllerBase
    {
        protected INarudzba _service { get; set; }

        public NarudzbaController(INarudzba service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("sve")]
        public IEnumerable<NarudzbaDto> GetSveNarudzbe()
        {
            return _service.GetSveNarudzbe();
        }

        [HttpGet]
        [Route("narudzba/korisnik/{korisnikId}")]
        public IEnumerable<NarudzbaDto> GetNarudzbeByKorisnikId(string korisnikId)
        {
            return _service.GetNarudzbeByKorisnikId(korisnikId);
        }

        [HttpGet]
        [Route("status/{narudzbaId}")]
        public ActionResult<string> GetStatusNarudzbe(int narudzbaId)
        {
            var narudzba = _service.GetNarudzbaById(narudzbaId);

            if (narudzba == null)
                return Ok("Nema");


                return Ok(new { status = narudzba.StatusNarudzbe });
        }

        [HttpGet]
        [Route("vrijeme/{narudzbaId}")]
        public ActionResult GetVrijemeDostave(int narudzbaId)
        {
            var narudzba = _service.GetNarudzbaById(narudzbaId);

            if (narudzba == null)
                return Ok("Nema");

            return Ok(new { vrijemeDostave = narudzba.VrijemeDostave });
        }




        [HttpPost]
        [Route("add")]

        public async Task<IActionResult> AddNarudzbaAsync([FromBody] NarudzbaREST narudzbaREST)
        {
            try
            {
                NarudzbaDto narudzbaDto = new NarudzbaDto();
                narudzbaDto.KorisnikId = narudzbaREST.KorisnikId;
                narudzbaDto.UkupnaCijena = narudzbaREST.UkupnaCijena;
                narudzbaDto.NacinDostave = narudzbaREST.NacinDostave;
                narudzbaDto.NacinPlacanja = narudzbaREST.NacinPlacanja;
                narudzbaDto.StatusNarudzbe = "Kreirana";
                narudzbaDto.DatumKreiranja = DateTime.Now;
                narudzbaDto.VrijemeDostave = narudzbaREST.VrijemeDostave;
                narudzbaDto.Grad = narudzbaREST.Grad;
                narudzbaDto.Ulica = narudzbaREST.Ulica;
                narudzbaDto.KucniBroj = narudzbaREST.KucniBroj;
                narudzbaDto.Stan = narudzbaREST.Stan;
                narudzbaDto.Kat = narudzbaREST.Kat;
                narudzbaDto.BrojKartice = narudzbaREST.BrojKartice;
                narudzbaDto.DatumIsteka = narudzbaREST.DatumIsteka;
                narudzbaDto.SigurnosniKod = narudzbaREST.SigurnosniKod;
                narudzbaDto.Napomena = narudzbaREST.Napomena;
                bool add_narudzba = await _service.AddNarudzbaAsync(narudzbaDto);
                if (add_narudzba)
                {
                    return Ok("Narudzba dodana");
                }
                else
                {
                    return Ok("Narudzba nije dodana");
                }
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("Greška pri spremanju narudžbe: " + ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditStatusNarudzbe([FromBody] NarudzbaUpdateREST narudzbaREST)
        {
            try
            {
                NarudzbaDto narudzbaDto = new NarudzbaDto();
                narudzbaDto.Id = narudzbaREST.Id;
                narudzbaDto.StatusNarudzbe = narudzbaREST.StatusNarudzbe;
                bool update_narudzba = await _service.EditStatusNarudzbe(narudzbaDto);
                if (update_narudzba)
                {
                    return Ok("Narudzba azurirana");
                }
                else
                {
                    return Ok("Narudzba nije azurirana");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        [Route("edit-zaposlenik")]
        public async Task<IActionResult> EditZaposlenik([FromBody] NarudzbaUpdateZaposlenik narudzbaREST)
        {
            try
            {
                NarudzbaDto narudzbaDto = new NarudzbaDto();
                narudzbaDto.Id = narudzbaREST.Id;
                narudzbaDto.ZaposlenikImePrezime = narudzbaREST.ZaposlenikImePrezime;
                bool update_narudzba = await _service.DodajZaposlenika(narudzbaDto);
                if (update_narudzba)
                {
                    return Ok("Narudzba azurirana");
                }
                else
                {
                    return Ok("Narudzba nije azurirana");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        // Kad se briše korisnik da se id korisnika prebaci na null
        [HttpPut("promjeniId")]
        public async Task<IActionResult> PromjeniKorisnikId([FromBody] narudzbaUpdateKorisnik narudzbaREST)
        {
            try
            {
                bool uspjeh = await _service.BrisanjeProfila(narudzbaREST.KorisnikId);

                return Ok(uspjeh ? "KorisnikId postavljen na null za sve narudžbe" : "Nije ažurirano");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }



    }
}
