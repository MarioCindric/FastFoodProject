using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service.Common
{
    public interface INarudzbaJeloService
    {
        IEnumerable<NarudzbaJeloDto> GetStavkeByNarudzbaId(int narudzbaId);
        Task<bool> AddNarudzbaJeloAsync(NarudzbaJeloDto dto);
    }
}
