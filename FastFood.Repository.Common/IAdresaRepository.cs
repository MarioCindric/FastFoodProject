using FastFood.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Repository.Common
{
    public interface IAdresaRepository
    {
        IEnumerable<AdresaDto> GetAdreseByKorisnikId(string korisnikId);
        AdresaDto GetAdreseById(int adresaId);
        Task<bool> AddAdresaAsync(AdresaDto adresaDto);
        Task<bool> UpdateAdresaAsync(AdresaDto adresaDto);
        Task<bool> DeleteAdresaAsync(int adresaId);

    }
}
