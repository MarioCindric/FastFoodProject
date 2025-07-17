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
    public class JeloRepository : IJeloRepository
    {
        private readonly AppDbContext _context;
        private IRepositoryMappingService _mapper;

        public JeloRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       
        public IEnumerable<JeloDto> GetAllJela()
        {
                var jela = _context.Jela.Include(j => j.Kategorija).ToList().OrderBy(j => j.KategorijaId).ToList();
            return jela.Select(e => _mapper.Map<JeloDto>(e)).ToList();
        }


        public JeloDto GetJeloById(int jeloId)
        {
            var jelo = _context.Jela
            .Include(j => j.Kategorija)
            .FirstOrDefault(j => j.Id == jeloId);
            return _mapper.Map<JeloDto>(jelo);

        }

        public async Task<bool> AddJeloAsync(JeloDto jelo)
        {
            var entity = _mapper.Map<Jelo>(jelo);
            var result = await _context.Jela.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateJeloAsync(JeloDto jelo)
        {
            var j = await _context.Jela.FindAsync(jelo.Id);
            if(j!=null)
            {
                j.Naziv = jelo.Naziv;
                j.Opis = jelo.Opis;
                j.Cijena = jelo.Cijena;
                j.Dostupno = jelo.Dostupno;
                j.SlikaUrl = jelo.SlikaUrl;
                j.Popust = jelo.Popust;
                j.KategorijaId = jelo.KategorijaId;
                _context.Jela.Update(j);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteJeloAsync(int jeloId)
        {

            var j = await _context.Jela.FindAsync(jeloId);
            if(j!=null)
            {
                _context.Jela.Remove(j);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

       
    }
}
