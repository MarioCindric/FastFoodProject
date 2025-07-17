using FastFood.Model;
using FastFood.Repository.Common;
using FastFoodIdentity.DAL;
using FastFoodRepository.Automapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FastFoodRepository
{
    public class KategorijaJeloRepository : IKategorijaJeloRepository
    {
        private readonly AppDbContext _context;
        private IRepositoryMappingService _mapper;

        public KategorijaJeloRepository(AppDbContext context, IRepositoryMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        IEnumerable<KategorijaJeloDto> IKategorijaJeloRepository.GetAllKategorije()
        {
            var kategorije = _context.Kategorije.ToList();
            return kategorije.Select(e => _mapper.Map<KategorijaJeloDto>(e)).ToList();
        }
    }
}
