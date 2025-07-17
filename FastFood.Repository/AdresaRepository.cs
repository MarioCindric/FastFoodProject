using FastFood.DAL.DataModel;
using FastFood.Model;
using FastFood.Repository.Common;
using FastFoodIdentity.DAL;
using FastFoodRepository.Automapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodRepository
{
    public class AdresaRepository : IAdresaRepository
    {
        private readonly AppDbContext _context;
        private IRepositoryMappingService _mapper;

        public AdresaRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<AdresaDto> GetAdreseByKorisnikId(string korisnikId)
        {
            var adrese = _context.Adrese
                .Where(a => a.KorisnikId == korisnikId)
                .ToList();
            
            return adrese.Select(a => _mapper.Map<AdresaDto>(a)).ToList();
        }

        public AdresaDto GetAdreseById(int adresaId)
        {
            var adresa = _context.Adrese.Find(adresaId);
            return _mapper.Map<AdresaDto>(adresa);
        }

        public async Task<bool> AddAdresaAsync(AdresaDto adresaDto)
        {
            var entity = _mapper.Map<Adresa>(adresaDto);
            var result = await _context.Adrese.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAdresaAsync(AdresaDto adresaDto)
        {
            var adresa = await _context.Adrese.FindAsync(adresaDto.Id);
            if (adresa != null)
            {
                adresa.Grad = adresaDto.Grad;
                adresa.Ulica = adresaDto.Ulica;
                adresa.BrojStana = adresaDto.BrojStana;
                adresa.Kat = adresaDto.Kat;
                _context.Adrese.Update(adresa);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAdresaAsync(int adresaId)
        {
            var adresa = await _context.Adrese.FindAsync(adresaId);
            if (adresa != null)
            {
                _context.Adrese.Remove(adresa);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
