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
                    ));
            CreateMap<SectionVM, Section>().ForMember(dest => dest.DepartureStation,
                opt => opt.Ignore())
                .ForMember(dest => dest.DestinationStation,
                opt => opt.Ignore());

            CreateMap<Station, StationVM>();
            CreateMap<AspNetUser, UserVM>();
            CreateMap<Train, TrainVM>();
            CreateMap<TrainVM, Train>();
        }
    }
}
