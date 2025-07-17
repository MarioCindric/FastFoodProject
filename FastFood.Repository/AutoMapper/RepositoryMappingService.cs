using AutoMapper;
using FastFood.DAL.DataModel;
using FastFood.Model;
using FastFoodIdentity.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodRepository.Automapper
{
    public class RepositoryMappingService : IRepositoryMappingService
    {
        public Mapper mapper;
        public RepositoryMappingService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Jelo, JeloDto>()
                    .ForMember(dest => dest.NazivKategorije, opt => opt.MapFrom(src => src.Kategorija.Naziv));
                cfg.CreateMap<JeloDto, Jelo>();
                cfg.CreateMap<KategorijaJela, KategorijaJeloDto>().ReverseMap();
                cfg.CreateMap<Adresa, AdresaDto>().ReverseMap();
                cfg.CreateMap<KreditnaKartica, KreditnaKarticaDto>().ReverseMap();
                cfg.CreateMap<Narudzba, NarudzbaDto>().ReverseMap();
                cfg.CreateMap<OcjenaJela, OcjenaJelaDto>().ReverseMap();
                cfg.CreateMap<OcjenaNarudzbe, OcjenaNarudzbaDto>().ReverseMap();
                cfg.CreateMap<NarudzbaJelo, NarudzbaJeloDto>()
                    .ForMember(dest => dest.NazivJela, opt => opt.MapFrom(src => src.Jelo.Naziv));
                cfg.CreateMap<NarudzbaJeloDto, NarudzbaJelo>();
            });
            mapper = new Mapper(config);
        }
        public TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }
    }
}
