using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service
{
    public class NarudzbaService : INarudzba
    {
        private readonly INarudzbaRepository _repo;

        public NarudzbaService(INarudzbaRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<NarudzbaDto> GetSveNarudzbe()
        {
            return _repo.GetSveNarudzbe();
        }

        public IEnumerable<NarudzbaDto> GetNarudzbeByKorisnikId(string korisnikId)
        {
            return _repo.GetNarudzbeByKorisnikId(korisnikId);
        }

        public async Task<bool> AddNarudzbaAsync(NarudzbaDto dto)
        {
            return await _repo.AddNarudzbaAsync(dto);
        }

        public async Task <bool> EditStatusNarudzbe(NarudzbaDto dto)
        {
            return await _repo.EditStatusNarudzbe(dto);
        }

        public async Task<bool> DodajZaposlenika(NarudzbaDto dto)
        {
            return await _repo.DodajZaposlenika(dto);
        }

        public NarudzbaDto GetNarudzbaById(int narudzbaId)
        {
            return  _repo.GetNarudzbaById(narudzbaId);
        }

        public Task<bool> BrisanjeProfila(string korisnikId)
        {
            return  _repo.BrisanjeProfila(korisnikId);
        }
    }
}
