using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Service.Common
{
    public interface IKategorijaJelo
    {
        IEnumerable<KategorijaJeloDto> GetAllKategorije();
    }
}
