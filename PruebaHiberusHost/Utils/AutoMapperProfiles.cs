using AutoMapper;
using PruebaHiberusHost.DTOs;
using PruebaHiberusHost.Entities;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Origen, Destino
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();
            CreateMap<Sum, SumDTO>();
            CreateMap<SumDTO, Sum>();
        }
    }
}
