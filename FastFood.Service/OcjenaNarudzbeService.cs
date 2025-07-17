using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System.Threading.Tasks;

namespace FastFood.Service
{
    public class OcjenaNarudzbeService : IOcjenaNarudzbeService
    {
        private readonly IOcjenaNarudzbeRepository _repo;

        public OcjenaNarudzbeService(IOcjenaNarudzbeRepository repo)
        {
            _repo = repo;
        }

        public async Task<OcjenaNarudzbaDto> GetByNarudzbaIdAsync(int narudzbaId)
        {
            return await _repo.GetByNarudzbaIdAsync(narudzbaId);
        }

        public async Task<bool> AddAsync(OcjenaNarudzbaDto dto)
        {
            return await _repo.AddAsync(dto);
        }
    }
}
