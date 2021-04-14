using AutoMapper;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;

namespace SportsSocialNetwork
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Sport, SportViewModel>();
            CreateMap<SportDtoModel, Sport>();
        }
    }
}
