using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace FastFood.Service
{
    public class KreditnaKarticaService : IKreditnaKartica
    {
        private readonly IKreditnaKarticaRepository _repo;

        public KreditnaKarticaService(IKreditnaKarticaRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<KreditnaKarticaDto> GetKarticeByKorisnikId(string korisnikId)
        {
            return _repo.GetKarticeByKorisnikId(korisnikId);
        }

        public Task<bool> UpdateKarticaAsync(KreditnaKarticaDto dto)
        {
            return _repo.UpdateKarticaAsync(dto);
        }

        public Task<bool> AddKarticaAsync(KreditnaKarticaDto dto)
        {
            return _repo.AddKarticaAsync(dto);
        }

        public Task<bool> DeleteKarticaAsync(int karticaId)
        {
            return _repo.DeleteKarticaAsync(karticaId);
        }
    }
}
