using FastFood.DAL.DataModel;
using FastFood.Model;
using FastFood.Repository.Common;
using FastFoodIdentity.DAL;
using FastFoodRepository.Automapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodRepository
{
    public class NarudzbaJeloRepository : INarudzbaJeloRepository
    {
        private readonly AppDbContext _context;
        private IRepositoryMappingService _mapper;

        public NarudzbaJeloRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<NarudzbaJeloDto> GetStavkeByNarudzbaId(int narudzbaId)
        {
            var narudzbaJelo = _context.NarudzbeJela
                .Where(nj => nj.NarudzbaId == narudzbaId)
                .Include(nj => nj.Jelo)
                .ToList();

            return narudzbaJelo.Select(e => _mapper.Map<NarudzbaJeloDto>(e)).ToList();
        }

        public async Task<bool> AddNarudzbaJeloAsync(NarudzbaJeloDto narudzbaJeloDto)
        {

            var entity = _mapper.Map<NarudzbaJelo>(narudzbaJeloDto);
            var result = await _context.NarudzbeJela.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;


        }
    }
}
