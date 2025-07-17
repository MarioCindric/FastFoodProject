using FastFood.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Service.Common
{
    public interface IOcjenaNarudzbeService
    {
        Task<OcjenaNarudzbaDto> GetByNarudzbaIdAsync(int narudzbaId);
        Task<bool> AddAsync(OcjenaNarudzbaDto dto);
    }
}
