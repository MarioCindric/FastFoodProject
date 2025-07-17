using FastFood.Model;
using FastFood.Service.Common;
using FastFood.WebAPI.RESTModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NarudzbaJeloController : ControllerBase
    {
        protected INarudzbaJeloService _service {  get; set; }

        public NarudzbaJeloController(INarudzbaJeloService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("narudzba/{narudzbaId}")]
        public IEnumerable<NarudzbaJeloDto> GetStavkeByNarudzbaId(int narudzbaId)
        {
            IEnumerable<NarudzbaJeloDto> stavke = _service.GetStavkeByNarudzbaId(narudzbaId);
            return stavke;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddNarudzbaJeloAsync([FromBody] NarudzbaJeloREST narudzbaJeloREST)
        {
            try
            {
                NarudzbaJeloDto narudzbaJeloDto = new NarudzbaJeloDto();
                narudzbaJeloDto.JeloId = narudzbaJeloREST.JeloId;
                narudzbaJeloDto.NarudzbaId = narudzbaJeloREST.NarudzbaId;
                narudzbaJeloDto.Kolicina = narudzbaJeloREST.Kolicina;
                narudzbaJeloDto.Napomena = narudzbaJeloREST.Napomena;
                narudzbaJeloDto.Cijena = narudzbaJeloREST.Cijena;

                bool add_narudzba = await _service.AddNarudzbaJeloAsync(narudzbaJeloDto);
                if (add_narudzba)
                {
                    return Ok("NarudzbaJelo dodana");
                }
                else
                {
                    return Ok("NarudzbaJelo nije dodana");
                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Greška pri spremanju narudžbe: " + ex.Message, ex);
            }
        }

    }
}
