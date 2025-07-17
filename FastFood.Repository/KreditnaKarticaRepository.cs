using FastFood.DAL.DataModel;
using FastFood.Model;
using FastFood.Repository.Common;
using FastFoodIdentity.DAL;
using FastFoodRepository.Automapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodRepository
{
    public class KreditnaKarticaRepository : IKreditnaKarticaRepository
    {
        private readonly AppDbContext _context;
        private IRepositoryMappingService _mapper;

        public KreditnaKarticaRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<KreditnaKarticaDto> GetKarticeByKorisnikId(string korisnikId)
        {
            var kartice = _context.Kartice
                .Where(k => k.KorisnikId == korisnikId)
                .ToList();
            return kartice.Select(k => _mapper.Map<KreditnaKarticaDto>(k)).ToList();
        }
        public KreditnaKarticaDto GetKarticaById(int karticaId)
        {
            var kartica = _context.Kartice.Find(karticaId);
            return _mapper.Map<KreditnaKarticaDto>(kartica);
        }

        public async Task<bool> AddKarticaAsync(KreditnaKarticaDto dto)
        {
            try
            {
                var entity = _mapper.Map<KreditnaKartica>(dto);
                var result = await _context.Kartice.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateKarticaAsync(KreditnaKarticaDto dto)
        {
            try
            {
                var kartica = await _context.Kartice.FindAsync(dto.Id);
                if (kartica != null)
                {
                    kartica.BrojKartice = dto.BrojKartice;
                    kartica.DatumIsteka = dto.DatumIsteka;
                    kartica.SigurnosniKod = dto.SigurnosniKod;
                    _context.Kartice.Update(kartica);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteKarticaAsync(int karticaId)
        {
            try
            {
                var kartica = await _context.Kartice.FindAsync(karticaId);
                if (kartica != null)
                {
                    _context.Kartice.Remove(kartica);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
