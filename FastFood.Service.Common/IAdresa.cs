using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service.Common
{
    public interface IAdresa
    {
        IEnumerable<AdresaDto> GetAdreseByKorisnikId(string korisnikId);
        AdresaDto GetAdreseById(int adresaId);

        Task<bool> AddAdresaAsync(AdresaDto adresaDto);
        Task<bool> UpdateAdresaAsync(AdresaDto adresaDto);
        Task<bool> DeleteAdresaAsync(int adresaId);
    }
}
