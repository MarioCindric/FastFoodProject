using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service
{
    public class NarudzbaJeloService : INarudzbaJeloService
    {
        private readonly INarudzbaJeloRepository _repo;

        public NarudzbaJeloService(INarudzbaJeloRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<NarudzbaJeloDto> GetStavkeByNarudzbaId(int narudzbaId)
        {
            return _repo.GetStavkeByNarudzbaId(narudzbaId);
        }

        public async Task<bool> AddNarudzbaJeloAsync(NarudzbaJeloDto dto)
        {
            return await _repo.AddNarudzbaJeloAsync(dto);
        }
    }
}
