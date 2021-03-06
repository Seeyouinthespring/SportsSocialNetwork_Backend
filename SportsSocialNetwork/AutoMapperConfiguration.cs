using AutoMapper;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Business.Enums;
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
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.FromBase64String(src.Photo) : null))
                .ForMember(dest => dest.Sports, opt => opt.MapFrom(src => src.SportsIds));
            CreateMap<long, PlaygroundSportConnection>()
                .ForMember(dest => dest.SportId, opt => opt.MapFrom(x => x));

            CreateMap<ApplicationUser, ApplicationUserBaseViewModel>()
                .ForMember(dest => dest.Photo, src => src.MapFrom(x => x.Photo != null ? Convert.ToBase64String(x.Photo) : null))
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


            CreateMap<RentRequest, RentRequestFullViewModel>()
                .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(x => x.Playground.Name))
                .ForMember(dest => dest.PlaygroundId, opt => opt.MapFrom(x => x.Playground.Id))
                .ForMember(dest => dest.PlaygroundCity, opt => opt.MapFrom(x => x.Playground.City))
                .ForMember(dest => dest.PlaygroundStreet, opt => opt.MapFrom(x => x.Playground.Street))
                .ForMember(dest => dest.PlaygroundHouseNumber, opt => opt.MapFrom(x => x.Playground.HouseNumber))
                .ForMember(dest => dest.PlaygroundPhoto, opt => opt.MapFrom(x => x.Playground.Photo != null ? Convert.ToBase64String(x.Playground.Photo) : null))
                .ForMember(dest => dest.PersonFirstName, opt => opt.MapFrom(x => x.Renter == null ? x.Playground.ResponsiblePerson.FirstName : x.Renter.FirstName))

                .ForMember(dest => dest.PersonLastName, opt => opt.MapFrom(x => x.Renter == null ? x.Playground.ResponsiblePerson.LastName : x.Renter.LastName))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(x => x.Renter == null ? x.Playground.ResponsiblePerson.Id : x.Renter.Id))
                .ForMember(dest => dest.PersonGender, opt => opt.MapFrom(x => x.Renter == null ? x.Playground.ResponsiblePerson.Gender : x.Renter.Gender))
                .ForMember(dest => dest.PersonPhoto, opt => opt.MapFrom(x =>
                        (x.Renter == null && x.Playground.ResponsiblePerson.Photo != null) ?
                        Convert.ToBase64String(x.Playground.ResponsiblePerson.Photo) :
                        (x.Renter != null && x.Renter.Photo != null) ?
                        Convert.ToBase64String(x.Renter.Photo) : null));

            CreateMap<RentRequest, RentRequestShortViewModel>()
                .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(src => src.Playground.Name));
            CreateMap<RentRequest, RentRequestViewModel>()
                .ForMember(dest => dest.Playground, opt => opt.MapFrom(x => x.Playground))
                //.ForMember(dest => dest.Renter.RentRequestId, opt => opt.MapFrom(src => src.Id));
                .ForMember(dest => dest.Renter, opt => opt.MapFrom(x => x.Renter));
            CreateMap<RentRquestDtoModel, RentRequest>();

            CreateMap<ApplicationUser, ApplicationUserRenterViewModel>()
                .ForMember(dest => dest.Photo, src => src.MapFrom(x => x.Photo != null ? Convert.ToBase64String(x.Photo) : null))
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

            CreateMap<ApplicationUser, ApplicationUserMessageViewModel>()
                .ForMember(dest => dest.Photo, src => src.MapFrom(x => x.Photo != null ? Convert.ToBase64String(x.Photo) : null));

            CreateMap<ConfirmedRent, RentViewModel>();

            CreateMap<Message, MessageViewModel>();
            CreateMap<MessageDtoModel, Message>();

            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentDtoModel, Comment>();

            CreateMap<ContactInformation, ContactInformationViewModel>();
            CreateMap<ContactInformationDtoModel, ContactInformation>();

            CreateMap<Playground, PlaygroundSummaryInfoViewModel>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.ToBase64String(src.Photo) : null))
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.ContactInformation ?? src.ResponsiblePerson.ContactInformation))
                .ForMember(dest => dest.SportsProvided, opt => opt.MapFrom(src => src.Sports.Select(x => x.Sport)));

            CreateMap<PersonalActivity, PersonalActivityViewModel>();
            CreateMap<PersonalActivityDtoModel, PersonalActivity>();

            CreateMap<Playground, PlaygroundShortViewModel>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.ToBase64String(src.Photo) : null))
                .ForMember(dest => dest.Sports, opt => opt.MapFrom(src => src.Sports.Select(x => x.Sport.Name)));

            CreateMap<ConfirmedRent, CommonActivityViewModel>()
                .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(src => src.Playground.Name))
                .ForMember(dest => dest.IsExecuted, opt => opt.MapFrom(src => src.IsExecuted))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ActivityType.Rent));
            CreateMap<ConfirmedRent, RentShortViewModel>()
                .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(src => src.Playground.Name));

            CreateMap<PersonalActivity, CommonActivityViewModel>()
                .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(src => src.Playground.Name))
                .ForMember(dest => dest.IsExecuted, opt => opt.MapFrom(src => src.IsVisited))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ActivityType.PersonalActivity));

            CreateMap<AppointmentVisiting, CommonActivityViewModel>()
                .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(src => src.Appointment.Playground.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Appointment.Date))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Appointment.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Appointment.EndTime))
                .ForMember(dest => dest.IsExecuted, opt => opt.MapFrom(src => src.Status == (byte)VisitingStatus.ExactlyVisit))
                .ForMember(dest => dest.PlaygroundId, opt => opt.MapFrom(src => src.Appointment.PlaygroundId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ActivityType.Appointment));

            CreateMap<ApplicationUser, ProfileViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo != null ? Convert.ToBase64String(src.Photo) : null))
                .ForMember(dest => dest.Age, opt => opt.MapFrom
                    (
                        src => src.DateOfBirth > DateTime.Now.AddYears(DateTime.Now.Year - src.DateOfBirth.Year) ?
                        DateTime.Now.Year - src.DateOfBirth.Year - 1 :
                        DateTime.Now.Year - src.DateOfBirth.Year
                ))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ContactInformation.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ContactInformation.Email))
                .ForMember(dest => dest.Vk, opt => opt.MapFrom(src => src.ContactInformation.Vk))
                .ForMember(dest => dest.Instagram, opt => opt.MapFrom(src => src.ContactInformation.Instagram));

            CreateMap<Appointment, AppointmentShortViewModel>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Playground.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Playground.Street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.Playground.HouseNumber))
                .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(src => src.Playground.Name))
                .ForMember(dest => dest.InitiatorFirstName, opt => opt.MapFrom(src => src.Initiator.FirstName))
                .ForMember(dest => dest.InitiatorLastName, opt => opt.MapFrom(src => src.Initiator.LastName))
                .ForMember(dest => dest.Sport, opt => opt.MapFrom(src => src.Sport.Name))
                .ForMember(dest => dest.CurrentVisitors, opt => opt.MapFrom(src => src.Visits.Count()));

            CreateMap<Appointment, SingleAppointmentViewModel>()
                .ForMember(dest => dest.PlaygroundPhoto,opt => opt.MapFrom(src => src.Playground.Photo != null ? Convert.ToBase64String(src.Playground.Photo) : null))
                .ForMember(dest => dest.Visits, opt => opt.MapFrom(src => src.Visits))
                .IncludeBase<Appointment, AppointmentShortViewModel>();

            CreateMap<AppointmentVisiting, VisitViewModel>()
                .ForMember(dest => dest.MemberLastName, opt => opt.MapFrom(src => src.Member.LastName))
                .ForMember(dest => dest.MemberFirstName, opt => opt.MapFrom(src => src.Member.FirstName))
                .ForMember
                (dest => dest.MemberAge, opt => opt.MapFrom
                    (src => src.Member.DateOfBirth > DateTime.Now.AddYears(DateTime.Now.Year - src.Member.DateOfBirth.Year) ? DateTime.Now.Year - src.Member.DateOfBirth.Year - 1 : DateTime.Now.Year - src.Member.DateOfBirth.Year))
                .ForMember(dest => dest.MemberGender, opt => opt.MapFrom(src => src.Member.Gender));

            CreateMap<Playground, PlaygroundMapViewModel>();
        }
    }
}
