using FastFood.Model;
using FastFood.Service.Common;
using FastFood.WebAPI.RESTModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Collections.Generic;



namespace FastFood.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcjenaJelaController : ControllerBase
    {
        protected IOcjenaJelaService _service;

        public OcjenaJelaController(IOcjenaJelaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("ocjene/{jeloId}/{korisnikId}")]
        public IActionResult GetOcjeneByJeloIdAndUserId(int jeloId, string korisnikId)
        {
            IEnumerable<OcjenaJelaDto> ocjene = _service.GetByJeloIdAndUserId(jeloId, korisnikId);
            return Ok(ocjene);
        }

        [HttpGet]
        [Route("ocjeneStatistika/{jeloId}")]
        public IActionResult GetOcjeneByJeloId(int jeloId)
        {
            IEnumerable<OcjenaJelaDto> ocjene = _service.GetByJeloId(jeloId);

            var grupirane = ocjene.GroupBy(o => o.Ocjena)
            .ToDictionary(g => g.Key, g => g.Count());

            var rezultat = Enumerable.Range(1, 5)
            .Select(i => new { Zvjezdice = i, Broj = grupirane.ContainsKey(i) ? grupirane[i] : 0 })
            .OrderByDescending(x => x.Zvjezdice)
            .ToList();

            return Ok(rezultat);

           
        }

        [HttpPost]
        [Route("ocijeni")]
        public async Task<IActionResult> OcijeniJelo([FromBody] OcjenaJelaREST rest)
        {
            try
            {
                OcjenaJelaDto dto = new OcjenaJelaDto
                {
                    JeloId = rest.JeloId,
                    Ocjena = rest.Ocjena,
                    KorisnikId = rest.KorisnikId
                };

                var postojeca = _service.GetByJeloIdAndUserId(dto.JeloId, dto.KorisnikId);

                bool rezultat;
                string poruka;

                if (!postojeca.Any())
                {
                    rezultat = await _service.AddAsync(dto);
                    poruka = rezultat ? "Ocjena dodana" : "Ocjena nije dodana";
                }
                else
                {
                    rezultat = await _service.UpdateAsync(dto);
                    poruka = rezultat ? "Ocjena ažurirana" : "Ocjena nije ažurirana";
                }

                return Ok(poruka);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri spremanju ocjene: " + ex.Message, ex);
            }
        }


        [HttpGet]
        [Route("prosjecna/{jeloId}")]
        public async Task<IActionResult> GetProsjecnaOcjena(int jeloId)
        {
            try
            {
                double? prosjek = await _service.GetProsjecnaOcjenaAsync(jeloId);
                return Ok(Math.Round(prosjek ?? 0.0, 1));

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri dohvaćanju prosječne ocjene: " + ex.Message, ex);
            }
        }

        [HttpDelete]
        [Route("delete/{jeloId}/{korisnikId}")]
        public async Task<IActionResult> DeleteOcjenaAsync(int jeloId, string korisnikId)
        {
            try
            {
                bool obrisana = await _service.DeleteOcjena(jeloId, korisnikId);
                if (obrisana)
                    return Ok("Ocjena obrisana");
                else
                    return NotFound("Ocjena nije pronađena");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Greška pri brisanju ocjene: " + ex.Message);
            }
        }


    }
}
