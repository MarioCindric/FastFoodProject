using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Repository.Common
{
    public interface IOcjenaNarudzbeRepository
    {
        Task<OcjenaNarudzbaDto> GetByNarudzbaIdAsync(int narudzbaId);
        Task<bool> AddAsync(OcjenaNarudzbaDto dto);
    }
}
