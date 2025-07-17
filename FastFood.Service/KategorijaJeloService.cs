using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System;
using System.Collections.Generic;
using System.Text;


namespace FastFood.Service
{
    public class KategorijaJeloService : IKategorijaJelo
    {
        private readonly IKategorijaJeloRepository _repo;

        public KategorijaJeloService(IKategorijaJeloRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<KategorijaJeloDto> GetAllKategorije()
        {
            return _repo.GetAllKategorije();
        }
    }
}
