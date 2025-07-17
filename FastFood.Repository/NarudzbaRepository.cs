using FastFood.DAL.DataModel;
using FastFood.Model;
using FastFood.Repository.Common;
using FastFoodIdentity.DAL;
using FastFoodRepository.Automapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FastFoodRepository
{
    public class NarudzbaRepository : INarudzbaRepository
    {
        private readonly AppDbContext _context;
        private IRepositoryMappingService _mapper;

        public NarudzbaRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddNarudzbaAsync(NarudzbaDto narudzbaDto)
        {
            
                var entity = _mapper.Map<Narudzba>(narudzbaDto);
                var result = await _context.Narudzbe.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            
            
        }

        public IEnumerable<NarudzbaDto> GetNarudzbeByKorisnikId(string korisnikId)
        {
            var narudzbe = _context.Narudzbe
                .Where(n => n.KorisnikId == korisnikId)
                .ToList();

            return narudzbe.Select(n => _mapper.Map<NarudzbaDto>(n)).ToList();
        }

        public IEnumerable<NarudzbaDto> GetSveNarudzbe()
        {
            var narudzbe = _context.Narudzbe
            .OrderByDescending(e => e.DatumKreiranja) 
            .ToList();

            return narudzbe
                .Select(e => _mapper.Map<NarudzbaDto>(e))
                .ToList();
        }

        public async Task<bool> EditStatusNarudzbe(NarudzbaDto dto)
        {
            try
            {
                var narudzba = await _context.Narudzbe.FindAsync(dto.Id);
                if (narudzba != null)
                {
                    narudzba.StatusNarudzbe = dto.StatusNarudzbe;
                    _context.Narudzbe.Update(narudzba);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DodajZaposlenika(NarudzbaDto dto)
        {
            try
            {
                var narudzba = await _context.Narudzbe.FindAsync(dto.Id);
                if (narudzba != null)
                {
                    narudzba.ZaposlenikImePrezime = dto.ZaposlenikImePrezime;
                    _context.Narudzbe.Update(narudzba);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public NarudzbaDto GetNarudzbaById(int narudzbaId)
        {
            var narudzba = _context.Narudzbe.FirstOrDefault(n => n.Id == narudzbaId);

            if (narudzba == null)
                return null;

            return _mapper.Map<NarudzbaDto>(narudzba);
        }

        public async Task<bool> BrisanjeProfila(string korisnikId)
        {
            var narudzbe = await _context.Narudzbe
                .Where(n => n.KorisnikId == korisnikId)
                .ToListAsync();

            foreach (var n in narudzbe)
                n.KorisnikId = null;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
