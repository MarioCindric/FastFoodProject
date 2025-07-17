using FastFood.Model;
using FastFood.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FastFood.WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class KategorijaJeloController : ControllerBase
    {

        protected IKategorijaJelo _service;

        public KategorijaJeloController(IKategorijaJelo service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("kategorije/sve")]
        public IEnumerable<KategorijaJeloDto> GetAllKategorijaJelo()
        {
            return _service.GetAllKategorije();
        }
    }
}
