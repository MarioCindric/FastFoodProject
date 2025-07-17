using FastFood.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Repository.Common
{
    public interface IKreditnaKarticaRepository
    {
        IEnumerable<KreditnaKarticaDto> GetKarticeByKorisnikId(string korisnikId);
        KreditnaKarticaDto GetKarticaById(int karticaId);
        Task<bool> AddKarticaAsync(KreditnaKarticaDto dto);
        Task<bool> UpdateKarticaAsync(KreditnaKarticaDto dto);
        Task<bool> DeleteKarticaAsync(int karticaId);

    }
}
