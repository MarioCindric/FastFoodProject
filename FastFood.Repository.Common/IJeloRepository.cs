using FastFood.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Repository.Common
{
    public interface IJeloRepository
    {
        IEnumerable<JeloDto> GetAllJela();
        JeloDto GetJeloById(int jeloId);

        Task<bool> AddJeloAsync(JeloDto jelo);
        Task<bool> UpdateJeloAsync(JeloDto jelo);
        Task<bool> DeleteJeloAsync(int jeloId);


    }
}
