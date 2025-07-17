using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service.Common
{
    public interface IOcjenaJelaService
    {
        IEnumerable<OcjenaJelaDto> GetByJeloIdAndUserId(int jeloId, string korisnikId);
        IEnumerable<OcjenaJelaDto> GetByJeloId(int jeloId);
        Task<bool> AddAsync(OcjenaJelaDto dto);
        Task<bool> UpdateAsync(OcjenaJelaDto dto);
        Task<double?> GetProsjecnaOcjenaAsync(int jeloId);
        Task<bool> DeleteOcjena(int jeloId, string korisnikId);
    }
}
