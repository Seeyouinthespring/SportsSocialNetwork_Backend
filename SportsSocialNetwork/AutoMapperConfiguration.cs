using AutoMapper;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using System;
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

            CreateMap<ApplicationUser, ApplicationUserBaseViewModel>()
                .ForMember
                (dest => dest.Age, opt => opt.MapFrom
                    (
                        src =>src.DateOfBirth > DateTime.Now.AddYears(DateTime.Now.Year - src.DateOfBirth.Year) ?
                        DateTime.Now.Year - src.DateOfBirth.Year - 1 :
                        DateTime.Now.Year - src.DateOfBirth.Year
                ));

            CreateMap<AppointmentVisiting, ApplicationUserVisitingViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Member.Id))
                .ForMember(dest => dest.VisitingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Member.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Member.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Member.UserName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Member.Gender))
                .ForMember(dest => dest.Age, opt => opt.MapFrom
                    (
                        src => src.Member.DateOfBirth > DateTime.Now.AddYears(DateTime.Now.Year - src.Member.DateOfBirth.Year) ?
                        DateTime.Now.Year - src.Member.DateOfBirth.Year - 1 :
                        DateTime.Now.Year - src.Member.DateOfBirth.Year
                ));

            CreateMap<AppointmentVisiting, VisitingBaseModel>();

            CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(dest => dest.PlannedVisits, opt => opt.MapFrom(src => src.Visits));
            CreateMap<AppointmentDtoModel, Appointment>();

            CreateMap<RentRequest, RentRequestViewModel>()
                .ForMember(dest => dest.Playground, opt => opt.MapFrom(x => x.Playground))
                //.ForMember(dest => dest.Renter.RentRequestId, opt => opt.MapFrom(src => src.Id));
                .ForMember(dest => dest.Renter, opt => opt.MapFrom(x => x.Renter));
            CreateMap<RentRquestDtoModel, RentRequest>();

            CreateMap<ApplicationUser, ApplicationUserRenterViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Age, opt => opt.MapFrom
                    (
                        src => src.DateOfBirth > DateTime.Now.AddYears(DateTime.Now.Year - src.DateOfBirth.Year) ?
                        DateTime.Now.Year - src.DateOfBirth.Year - 1 :
                        DateTime.Now.Year - src.DateOfBirth.Year
                ));

            CreateMap<ApplicationUser, ApplicationUserMessageViewModel>();

            CreateMap<ConfirmedRent, RentViewModel>();

            CreateMap<Message, MessageViewModel>();
            CreateMap<MessageDtoModel, Message>();
        }
    }
}
