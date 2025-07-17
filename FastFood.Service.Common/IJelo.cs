using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service.Common
{
    public interface IJelo
    {
        IEnumerable<JeloDto> GetAllJela();
        JeloDto GetJeloById(int jeloId);
        Task<bool> AddJeloAsync(JeloDto jelo);
        Task<bool> UpdateJeloAsync(JeloDto jelo);
        Task<bool> DeleteJeloAsync(int jeloId);
    }
}
