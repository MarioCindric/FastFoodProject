using FastFood.DAL.DataModel;
using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Service
{
    public class OcjenaJelaService : IOcjenaJelaService
    {
        private readonly IOcjenaJelaRepository _repo;

        public OcjenaJelaService(IOcjenaJelaRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<OcjenaJelaDto> GetByJeloIdAndUserId(int jeloId, string korisnikId)
        {
            return _repo.GetByJeloIdAndUserId(jeloId, korisnikId);
        }

        public IEnumerable<OcjenaJelaDto> GetByJeloId(int jeloId)
        {
            return _repo.GetByJeloId(jeloId);
        }

        public async Task<bool> AddAsync(OcjenaJelaDto dto)
        {
            return await _repo.AddAsync(dto);
        }

        public async Task<bool> UpdateAsync(OcjenaJelaDto dto)
        {
            return await _repo.UpdateAsync(dto);
        }

        public async Task<double?> GetProsjecnaOcjenaAsync(int jeloId)
        {
            return await _repo.GetProsjecnaOcjenaAsync(jeloId);
        }

        public async Task<bool> DeleteOcjena(int jeloId, string korisnikId)
        {
            return await _repo.DeleteOcjena(jeloId, korisnikId);
        }
    }
}
