using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service.Common
{
    public interface IKreditnaKartica
    {
        IEnumerable<KreditnaKarticaDto> GetKarticeByKorisnikId(string korisnikId);
        Task<bool> AddKarticaAsync(KreditnaKarticaDto dto);
        Task<bool> UpdateKarticaAsync(KreditnaKarticaDto dto);
        Task<bool> DeleteKarticaAsync(int karticaId);
    }
}
