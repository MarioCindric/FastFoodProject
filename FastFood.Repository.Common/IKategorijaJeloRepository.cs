using FastFood.Model;
using System.Collections.Generic;

namespace FastFood.Repository.Common
{
    public interface IKategorijaJeloRepository
    {
        IEnumerable<KategorijaJeloDto> GetAllKategorije();

    }
}
