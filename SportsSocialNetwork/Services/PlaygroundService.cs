using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class PlaygroundService : IPlaygroundService
    {
        private readonly ICommonRepository _commonRepository;

        public PlaygroundService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public virtual async Task<PlaygroundViewModel> CreateAsync(PlaygroundDtoModel model, string userId)
        {
            var entity = model.MapTo<Playground>();
            entity.ApplicationUserId = userId;

            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<PlaygroundViewModel> UpdateAsync(PlaygroundDtoModel model, long id, string userId)
        {
            Playground entity = await _commonRepository.FindByCondition<Playground>(x => x.Id == id)
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            entity = Mapper.Map(model, entity);
            entity.ApplicationUserId = userId;

             _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<List<PlaygroundViewModel>> GetAllAsync(string search = null)
        {
            List<Playground> entities = await _commonRepository.GetAll<Playground>()
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .ToListAsync();

            return entities.MapTo<List<PlaygroundViewModel>>();
        }

        public async Task<PlaygroundViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<Playground>(x => x.Id == id)
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<PlaygroundViewModel>();
        }

        public async Task DeleteAsync(long id)
        {
            await _commonRepository.DeleteAsync<Playground>(id);

            await _commonRepository.SaveAsync();
        }

        public async Task<PlaygroundViewModel> ApproveAsync(long id)
        {
            Playground entity = await _commonRepository.FindByCondition<Playground>(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            entity.IsApproved = true;

            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<PlaygroundSummaryInfoViewModel> GetSummaryInfoAsync(long id)
        {
            Playground entity = await _commonRepository.FindByCondition<Playground>(x => x.Id == id)
                .Include(x => x.ResponsiblePerson).ThenInclude(x => x.ContactInformation)
                .Include(x => x.ContactInformation)
                .Include(x => x.Comments).ThenInclude(x => x.Author)
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            return entity.MapTo<PlaygroundSummaryInfoViewModel>();
        }

        public async Task<List<TimingIntervalModel>> GetFreeTimingsAsync(long id, DateTime date)
        {
            List<ConfirmedRent> rents = await _commonRepository.FindByCondition<ConfirmedRent>(x => x.PlaygroundId == id && 
                    x.Date == date && 
                    x.Playground.ClosedTill == null && 
                    x.Playground.IsApproved && 
                    x.Playground.IsCommercial)
                .Include(x => x.Playground)
                .OrderBy(x => x.StartTime)
                .ToListAsync();

            if (rents == null || !rents.Any()) return null;

            TimeSpan open = rents.First().Playground.OpenTime.Value;
            TimeSpan close = rents.First().Playground.CloseTime.Value;
            List<TimingIntervalModel> results = new List<TimingIntervalModel>(rents.Count() + 1);

            if (open != rents.First().StartTime) 
                results.Add(new TimingIntervalModel { StartTime = open, EndTime = rents.First().StartTime });
            
            for (int i = 0; i < rents.Count() - 1; i++)
                if (rents[i-2].StartTime != rents[i].EndTime)
                    results.Add(new TimingIntervalModel { StartTime = rents[i].EndTime, EndTime = rents[i + 1].StartTime });

            if (close != rents.Last().EndTime)
                results.Add(new TimingIntervalModel { StartTime = rents.Last().EndTime, EndTime = close });

            return results;
        }

        public async Task<VisitorsNumberViewModel> GetVisitorsNumberAsync(long id, DateTime date, TimeSpan time)
        {
            int number = await _commonRepository.FindByCondition<AppointmentVisiting>(x => x.Appointment.PlaygroundId == id &&
                    x.Appointment.Date == date &&
                    x.Appointment.StartTime <= time && x.Appointment.EndTime >= time)
                .CountAsync();

            number += await _commonRepository.FindByCondition<PersonalActivity>(x => x.PlaygroundId == id &&
                    x.Date == date &&
                    x.StartTime <= time &&
                    x.EndTime >= time)
                .CountAsync();

            return new VisitorsNumberViewModel { Number = number };
        }
    }
}
