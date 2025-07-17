using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodRepository.Automapper
{
    public interface IRepositoryMappingService
    {
        TDestination Map<TDestination>(object source);
    }
}
