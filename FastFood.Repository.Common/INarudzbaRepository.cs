using FastFood.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Repository.Common
{
    public interface INarudzbaRepository
    {
        IEnumerable<NarudzbaDto> GetSveNarudzbe();

        IEnumerable<NarudzbaDto> GetNarudzbeByKorisnikId(string korisnikId);
        NarudzbaDto GetNarudzbaById(int narudzbaId);
        Task<bool> AddNarudzbaAsync(NarudzbaDto dto);
        Task<bool> EditStatusNarudzbe(NarudzbaDto dto);
        Task<bool> BrisanjeProfila(string korisnikId);
        Task<bool> DodajZaposlenika(NarudzbaDto dto);


    }
}
