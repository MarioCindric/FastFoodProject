using FastFood.Model;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service
{
    public class AdresaService : IAdresa
    {
        private readonly IAdresaRepository _repo;

        public AdresaService(IAdresaRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<AdresaDto> GetAdreseByKorisnikId(string korisnikId)
        {
            return _repo.GetAdreseByKorisnikId(korisnikId);
        }

        public AdresaDto GetAdreseById(int adresaId)
        {
            return _repo.GetAdreseById(adresaId);
        }

        public Task<bool> AddAdresaAsync(AdresaDto adresaDto)
        {
            return _repo.AddAdresaAsync(adresaDto);
        }

        public Task<bool> UpdateAdresaAsync(AdresaDto adresaDto)
        {
            return _repo.UpdateAdresaAsync(adresaDto);
        }

        public Task<bool> DeleteAdresaAsync(int adresaId)
        {
            return _repo.DeleteAdresaAsync(adresaId);
        }
    }
}
