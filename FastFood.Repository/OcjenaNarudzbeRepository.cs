using FastFood.Model;
using FastFood.Repository.Common;
using FastFoodIdentity.DAL;
using FastFoodIdentity.DAL.Model;
using FastFoodRepository.Automapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FastFood.Repository
{
    public class OcjenaNarudzbeRepository : IOcjenaNarudzbeRepository
    {
        private readonly AppDbContext _context;
        private readonly IRepositoryMappingService _mapper;

        public OcjenaNarudzbeRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OcjenaNarudzbaDto> GetByNarudzbaIdAsync(int narudzbaId)
        {
            var entity = await _context.OcjeneNarudzbi
                .FirstOrDefaultAsync(o => o.NarudzbaId == narudzbaId);

            return entity == null ? null : _mapper.Map<OcjenaNarudzbaDto>(entity);
        }

        public async Task<bool> AddAsync(OcjenaNarudzbaDto dto)
        {
            var entity = _mapper.Map<OcjenaNarudzbe>(dto);
            await _context.OcjeneNarudzbi.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
