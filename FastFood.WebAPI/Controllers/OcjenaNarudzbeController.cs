using FastFood.Model;
using FastFood.Service.Common;
using FastFood.WebAPI.RESTModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FastFood.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcjenaNarudzbeController : ControllerBase
    {
        private readonly IOcjenaNarudzbeService _service;

        public OcjenaNarudzbeController(IOcjenaNarudzbeService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("dodaj")]
        public async Task<IActionResult> OcijeniNarudzbu([FromBody] OcjenaNarudzbaREST rest)
        {
            try
            {
                var postojeca = await _service.GetByNarudzbaIdAsync(rest.NarudzbaId);

                if (postojeca != null)
                {
                    return BadRequest("Narudžba je već ocijenjena.");
                }

                OcjenaNarudzbaDto dto = new OcjenaNarudzbaDto
                {
                    NarudzbaId = rest.NarudzbaId,
                    Ocjena = rest.Ocjena,
                    Komentar = rest.Komentar
                };


                var rezultat = await _service.AddAsync(dto);

                if (rezultat)
                {
                    return Ok("Ocjena spremljena.");
                }
                else
                {
                    return BadRequest("Greška pri spremanju ocjene.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri obradi ocjene narudžbe: " + ex.Message, ex);
            }
        }

        [HttpGet("{narudzbaId}")]
        public async Task<IActionResult> GetOcjenaNarudzbe(int narudzbaId)
        {
            try
            {
                var dto = await _service.GetByNarudzbaIdAsync(narudzbaId);

                if (dto != null)
                {
                    return Ok(dto);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri dohvaćanju ocjene narudžbe: " + ex.Message, ex);
            }
        }

    }
}
