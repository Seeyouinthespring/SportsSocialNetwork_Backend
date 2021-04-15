using AutoMapper;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using System.Linq;

namespace SportsSocialNetwork
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Sport, SportViewModel>();
            CreateMap<SportDtoModel, Sport>();

            CreateMap<Playground, PlaygroundViewModel>()
                .ForMember(dest => dest.SportsIds, opt => opt.MapFrom(src => src.Sports))
                .ForMember(dest => dest.SportsProvided, opt => opt.MapFrom(src => src.Sports.Select(x => x.Sport)));
            CreateMap<PlaygroundSportConnection, long>().ConvertUsing(x => x.SportId);
            CreateMap<PlaygroundDtoModel, Playground>()
                .ForMember(dest => dest.Sports, opt => opt.MapFrom(src => src.SportsIds));
            CreateMap<long, PlaygroundSportConnection>()
                .ForMember(dest => dest.SportId, opt => opt.MapFrom(x => x));
        }
    }
}
