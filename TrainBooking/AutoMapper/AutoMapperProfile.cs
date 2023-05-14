using AutoMapper;
using TrainBooking.Models.Entities;
using TrainBooking.ViewModels;

namespace TrainBooking.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Section, SectionVM>().ForMember(dest => dest.DepartureStation,
                opts => opts.MapFrom(
                    src => src.DepartureStation.City
                    ))
                .ForMember(dest => dest.DestinationStation,
                opts => opts.MapFrom(
                    src => src.DestinationStation.City
                    ))
                                .ForMember(dest => dest.EconomyClassCapacity,
                opts => opts.MapFrom(
                    src => src.Train.EconomyClassCapacity
                    ))
                .ForMember(dest => dest.BusinessClassCapacity,
                opts => opts.MapFrom(
                    src => src.Train.BusinessClassCapacity
                    ));
        }
    }
}
