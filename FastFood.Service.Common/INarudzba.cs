using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service.Common
{
    public interface INarudzba
    {
        IEnumerable<NarudzbaDto> GetSveNarudzbe();

        IEnumerable<NarudzbaDto> GetNarudzbeByKorisnikId(string korisnikId);
        Task<bool> AddNarudzbaAsync(NarudzbaDto dto);
        Task<bool> EditStatusNarudzbe(NarudzbaDto dto);
        Task<bool> DodajZaposlenika(NarudzbaDto dto);
        NarudzbaDto GetNarudzbaById(int narudzbaId);
        Task<bool> BrisanjeProfila(string korisnikId);
    }

}
