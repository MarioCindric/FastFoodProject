using FastFood.DAL.DataModel;
using FastFood.Model;
using FastFood.Service.Common;
using FastFood.WebAPI.RESTModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JeloController : ControllerBase
    {
        protected IJelo _service;

        public JeloController(IJelo service)
        {
            _service = service;
        }

        //[Authorize]
        [HttpGet]
        public IEnumerable<JeloDto> GetAll()
        {
            return _service.GetAllJela();
        }
        //[Authorize (Roles = "Zaposlenik")]
        [Authorize]
        [HttpGet]
        [Route("{jeloId}")]
        public ActionResult<JeloDto> GetById(int jeloId)
        {
            var jelo = _service.GetJeloById(jeloId);
            return jelo;
        }

        //[Authorize (Roles = "Zaposlenik")]
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddJeloAsync([FromBody] JeloREST jeloREST)
        {
            try
            {
                JeloDto jeloDto = new JeloDto();
                jeloDto.Naziv = jeloREST.Naziv;
                jeloDto.Opis = jeloREST.Opis;
                jeloDto.Cijena = jeloREST.Cijena;
                jeloDto.Dostupno = jeloREST.Dostupno;
                jeloDto.SlikaUrl = jeloREST.SlikaUrl;
                jeloDto.Popust = jeloREST.Popust;
                jeloDto.KategorijaId = jeloREST.KategorijaId;
                bool add_jelo = await _service.AddJeloAsync(jeloDto);
                if(add_jelo)
                {
                    return Ok("Jelo dodano");
                }
                else
                {
                    return BadRequest();
                }
            }

            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri dodavanju jela: " + ex.Message, ex);
            }
        }

        //[Authorize (Roles = "Zaposlenik")]
        [Authorize]
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> UpdateJeloAsync([FromBody] JeloREST jeloREST)
        {

            try
            {
                JeloDto jeloDto = new JeloDto();
                jeloDto.Id = jeloREST.Id;
                jeloDto.Naziv = jeloREST.Naziv;
                jeloDto.Opis = jeloREST.Opis;
                jeloDto.Cijena = jeloREST.Cijena;
                jeloDto.Dostupno = jeloREST.Dostupno;
                jeloDto.SlikaUrl = jeloREST.SlikaUrl;
                jeloDto.Popust = jeloREST.Popust;
                jeloDto.KategorijaId = jeloREST.KategorijaId;
                bool edit_jelo = await _service.UpdateJeloAsync(jeloDto);
                if (edit_jelo)
                {
                    return Ok("Jelo azurirano");
                }
                else
                {
                    return BadRequest();
                }
            }

            catch (Exception ex)
            {
                throw new InvalidOperationException("Greška pri uređivanju jela: " + ex.Message, ex);
            }

        }
        //[Authorize (Roles = "Zaposlenik")]
        [Authorize]
        [HttpDelete("delete/{jeloId}")]

        public async Task<IActionResult> DeleteJelo(int jeloId)
        {
            try
            {
                bool obrisano = await _service.DeleteJeloAsync(jeloId);
                if (obrisano)
                    return Ok("Jelo obrisano");
                else
                    return NotFound("Jelo nije pronadeno");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Greška pri brisanju jela: " + ex.Message);
            }
        }
    }

}
