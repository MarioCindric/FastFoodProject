using FastFood.Model;
using FastFoodIdentity.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Repository.Common
{
    public interface IOcjenaJelaRepository
    {
        IEnumerable<OcjenaJelaDto> GetByJeloIdAndUserId(int jeloId, string korisnikId);
        IEnumerable<OcjenaJelaDto> GetByJeloId(int jeloId);
        Task<bool> AddAsync(OcjenaJelaDto dto);
        Task<bool> UpdateAsync(OcjenaJelaDto dto);
        Task<double?> GetProsjecnaOcjenaAsync(int jeloId);
        Task<bool> DeleteOcjena(int jeloId, string korisnikId);
    }
}
