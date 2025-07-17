using FastFood.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Repository.Common
{
    public interface INarudzbaJeloRepository
    {
        IEnumerable<NarudzbaJeloDto> GetStavkeByNarudzbaId(int narudzbaId);
        Task<bool> AddNarudzbaJeloAsync(NarudzbaJeloDto dto);

    }
}
