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

        public virtual async Task<AppointmentViewModel> CreateAsync(AppointmentDtoModel model, string userId)
        {
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

            return await GetAsync(entity.Id);
        }

        public async Task<AppointmentViewModel> UpdateAsync(AppointmentDtoModel model, long id, string userId)
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

        public async Task<List<AppointmentViewModel>> GetAllAsync(string search = null)
        {
            List<Appointment> entities = await _commonRepository.GetAll<Appointment>()
                .Include(x => x.Initiator)
                .Include(x => x.Visits).ThenInclude(x => x.Member)
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .ToListAsync();

            return entities.MapTo<List<AppointmentViewModel>>();
        }

        public async Task<AppointmentViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<Appointment>(x => x.Id == id)
                .Include(x => x.Initiator)
                .Include(x => x.Visits).ThenInclude(x => x.Member)
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<AppointmentViewModel>();
        }

        public async Task DeleteAsync(long id)
        {
            await _commonRepository.DeleteAsync<Appointment>(id);

            await _commonRepository.SaveAsync();
        }

        public async Task<List<AppointmentShortViewModel>> GetForPlaygroundAsync(long id, DateTime currentDate)
        {
            var entities = await _commonRepository.FindByCondition<Appointment>(x =>
                x.PlaygroundId == id && x.Date >= currentDate.Date && x.StartTime >= currentDate.TimeOfDay)
                .Take(10)
                .Select(x => new Appointment
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Date = x.Date,
                    ParticipantsQuantity = x.ParticipantsQuantity,
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
                    Visits = x.Visits.Select(y => new AppointmentVisiting 
                    {
                        Id = y.Id
                    }).ToList()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return entities.MapTo<List<AppointmentShortViewModel>>();
        }
    }
}
