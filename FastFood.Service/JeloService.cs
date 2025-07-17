using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service
{
    public class JeloService : IJelo
    {
        private readonly IJeloRepository _repo;

        public JeloService(IJeloRepository repo)
        {
            _repo = repo;
        }

      

        public IEnumerable<JeloDto> GetAllJela()
        {
            return _repo.GetAllJela();
        }

        public JeloDto GetJeloById(int jeloId)
        {
            return _repo.GetJeloById(jeloId);
        }

        public Task<bool> AddJeloAsync(JeloDto jelo)
        {
            return _repo.AddJeloAsync(jelo);
        }

        public Task<bool> UpdateJeloAsync(JeloDto jelo)
        {
            return _repo.UpdateJeloAsync(jelo);
        }

        public Task<bool> DeleteJeloAsync(int jeloId)
        {
            return _repo.DeleteJeloAsync(jeloId);
        }


    }
}
