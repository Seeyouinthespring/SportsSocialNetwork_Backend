using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Business.Enums;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SportsSocialNetwork.Services
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly ICommonRepository _commonRepository;

        public AppointmentsService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public virtual async Task<AppointmentAdditingError> CreateAsync(AppointmentDtoModel model, string userId)
        {

            var playground = await _commonRepository.FindByCondition<Playground>(x => x.Id == model.PlaygroundId)
                .Include(x => x.Sports)
                .ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();

            if (playground.IsCommercial)
                return AppointmentAdditingError.WrongPlayground;
            if (!playground.Sports.Select(x => x.SportId).ToList().Contains(model.SportId.Value))
                return AppointmentAdditingError.WrongSport;
            if (model.EndTime <= model.StartTime)
                return AppointmentAdditingError.WrongTimeInterval;
            if (playground.OpenTime != null && (playground.OpenTime > model.StartTime || playground.CloseTime < model.EndTime))
                return AppointmentAdditingError.WrongTimeInterval;

            var visits = await _commonRepository.FindByCondition<AppointmentVisiting>(x => x.MemberId == userId)
                .Include(x => x.Appointment)
                .ToListAsync();

            if (visits.Any(x => x.Appointment.Date.Date == model.Date.Value &&
                ((x.Appointment.StartTime == model.StartTime) ||
                (x.Appointment.EndTime == model.EndTime) ||
                (x.Appointment.StartTime < model.StartTime && x.Appointment.EndTime > model.StartTime) ||
                (x.Appointment.StartTime < model.EndTime && x.Appointment.EndTime > model.EndTime))))
                return AppointmentAdditingError.DuplicateAppointment;

            var entity = model.MapTo<Appointment>();

            entity.InitiatorId = userId;
            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            AppointmentVisiting visit = new AppointmentVisiting
            {
                AppointmentId = entity.Id,
                MemberId = userId,
                Status = (byte)VisitingStatus.ExactlyVisit
            };
            await _commonRepository.AddAsync(visit);
            await _commonRepository.SaveAsync();

            return AppointmentAdditingError.Ok;
        }

        public async Task<SingleAppointmentViewModel> UpdateAsync(AppointmentDtoModel model, long id, string userId)
        {
            var entity = await _commonRepository.FindByCondition<Appointment>(x => x.Id == id)
                .Include(x => x.Initiator)
                .Include(x => x.Visits).ThenInclude(x => x.Member)
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();
            
            if (entity == null) return null;

            entity = Mapper.Map(model, entity);
            entity.InitiatorId = userId;

            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<List<AppointmentShortViewModel>> GetAllAsync(bool? isActual, int? sportId, int? playgroundId, string userId, DateTime currentDate)
        {
            var entities = await _commonRepository.FindByCondition<Appointment>(x => 
                (playgroundId == null || x.PlaygroundId == playgroundId.Value) &&
                (sportId == null || x.SportId == sportId.Value))
                .SelectData(userId)
                .ToListAsync();
            entities = entities.Where(x => (isActual == null) || (currentDate <= x.Date.AddSeconds(x.StartTime.TotalSeconds)) == isActual.Value).ToList();
            return entities.MapTo<List<AppointmentShortViewModel>>();
        }

        public async Task<SingleAppointmentViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<Appointment>(x => x.Id == id)
                .SelectDataForOneAppointment()
                .FirstOrDefaultAsync();
            if (entity == null) return null;
            return entity.MapTo<SingleAppointmentViewModel>();
        }

        public async Task DeleteAsync(long id)
        {
            await _commonRepository.DeleteAsync<Appointment>(id);
            await _commonRepository.SaveAsync();
        }

        public async Task<List<AppointmentShortViewModel>> GetForPlaygroundAsync(long id, DateTime currentDate, string currentUserId)
        {
            var entities = await _commonRepository.FindByCondition<Appointment>(x =>
                x.PlaygroundId == id && x.Date >= currentDate.Date && (x.Date == currentDate && x.StartTime >= currentDate.TimeOfDay || x.Date != currentDate))
                .Take(10)
                .SelectData(currentUserId)
                .ToListAsync();

            foreach (var entity in entities)
                entity.Participation = entity.Visits.Any(x => x.MemberId == currentUserId);

            return entities.MapTo<List<AppointmentShortViewModel>>();
        }
    }

    internal static class AppointmentQueryHelper 
    {
        internal static IQueryable<Appointment> SelectData(this IQueryable<Appointment> query, string currentUserId)
        {
            return query
                .Select(x => new Appointment
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Date = x.Date,
                    ParticipantsQuantity = x.ParticipantsQuantity,
                    Participation = x.Visits.Any(x => x.MemberId == currentUserId),
                    Playground = new Playground
                    {
                        Name = x.Playground.Name,
                        City = x.Playground.City,
                        Street = x.Playground.Street,
                        HouseNumber = x.Playground.HouseNumber
                    },
                    Initiator = new ApplicationUser
                    {
                        FirstName = x.Initiator.FirstName,
                        LastName = x.Initiator.LastName,
                    },
                    Sport = new Sport
                    {
                        Name = x.Sport.Name
                    },
                    Visits = x.Visits.Select(y => new AppointmentVisiting
                    {
                        Id = y.Id,
                        MemberId = y.MemberId
                    }).ToList()
                })
                .OrderBy(x => x.Date);
        }

        internal static IQueryable<Appointment> SelectDataForOneAppointment(this IQueryable<Appointment> query)
        {
            return query
                .Select(x => new Appointment
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Date = x.Date,
                    ParticipantsQuantity = x.ParticipantsQuantity,
                    Description= x.Description,
                    InitiatorId = x.InitiatorId,
                    Playground = new Playground
                    {
                        Name = x.Playground.Name,
                        City = x.Playground.City,
                        Street = x.Playground.Street,
                        HouseNumber = x.Playground.HouseNumber,
                        Photo = x.Playground.Photo
                    },
                    Sport = new Sport
                    {
                        Name = x.Sport.Name
                    },
                    Visits = x.Visits.Select(y => new AppointmentVisiting
                    {
                        Id = y.Id,
                        MemberId = y.MemberId,
                        Status = y.Status,
                        Member = new ApplicationUser 
                        {
                            Id = y.Member.Id,
                            LastName = y.Member.LastName,
                            FirstName = y.Member.FirstName,
                            Gender = y.Member.Gender,
                            DateOfBirth = y.Member.DateOfBirth,
                            //Photo = y.Member.Photo
                        }
                    }).ToList()
                })
                .OrderBy(x => x.Date);
        }
    }
}
