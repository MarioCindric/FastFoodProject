using FastFood.Model;
using FastFood.Repository.Common;
using FastFoodIdentity.DAL;
using FastFoodRepository.Automapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FastFoodIdentity.DAL.Model;
using System.Collections.Generic;
using System;

namespace FastFood.Repository
{
    public class OcjenaJelaRepository : IOcjenaJelaRepository
    {
        private readonly AppDbContext _context;
        private readonly IRepositoryMappingService _mapper;

        public OcjenaJelaRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public IEnumerable<OcjenaJelaDto> GetByJeloIdAndUserId(int jeloId, string korisnikId)
        {
            var ocjene = _context.OcjenaJela
                .Where(o => o.JeloId == jeloId && o.KorisnikId == korisnikId)
                .ToList();

            return ocjene.Select(o => _mapper.Map<OcjenaJelaDto>(o)).ToList();
        }

        public IEnumerable<OcjenaJelaDto> GetByJeloId(int jeloId)
        {
            var ocjene = _context.OcjenaJela
                .Where(o => o.JeloId == jeloId)
                .ToList();

            return ocjene.Select(o => _mapper.Map<OcjenaJelaDto>(o)).ToList();
        }

        public async Task<bool> AddAsync(OcjenaJelaDto dto)
        {
            var entity = _mapper.Map<OcjenaJela>(dto);
            await _context.OcjenaJela.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(OcjenaJelaDto dto)
        {
            var entity = await _context.OcjenaJela
                .FirstOrDefaultAsync(o => o.JeloId == dto.JeloId && o.KorisnikId == dto.KorisnikId);

            if (entity == null) return false;

            entity.Ocjena = dto.Ocjena;
            _context.OcjenaJela.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<double?> GetProsjecnaOcjenaAsync(int jeloId)
        {
            return await _context.OcjenaJela
                .Where(o => o.JeloId == jeloId)
                .AverageAsync(o => (double?)o.Ocjena);
        }

        public async Task<bool> DeleteOcjena(int jeloId, string korisnikId)
        {
            try
            {
                var ocjena = await _context.OcjenaJela
                    .FirstOrDefaultAsync(o => o.JeloId == jeloId && o.KorisnikId == korisnikId);

                if (ocjena != null)
                {
                    _context.OcjenaJela.Remove(ocjena);
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

    }
}
