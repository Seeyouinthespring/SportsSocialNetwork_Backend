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
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            return entity.MapTo<PlaygroundSummaryInfoViewModel>();
        }

        public async Task<List<TimingIntervalModel>> GetFreeTimingsAsync(long id, DateTime date)
        {
            List<ConfirmedRent> rents = await _commonRepository.FindByCondition<ConfirmedRent>(x => x.PlaygroundId == id && 
                    x.Date.Date == date && 
                    x.Playground.ClosedTill == null && 
                    x.Playground.IsApproved && 
                    x.Playground.IsCommercial)
                .Include(x => x.Playground)
                .OrderBy(x => x.StartTime)
                .ToListAsync();

            if (rents == null || !rents.Any()) 
            {
                var playground = await _commonRepository.FindByIdAsync<Playground>(id);
                List<TimingIntervalModel> result = new List<TimingIntervalModel>();
                result.Add(new TimingIntervalModel 
                {
                    StartTime = playground.OpenTime ?? new TimeSpan(0,1,0),
                    EndTime = playground.CloseTime ?? new TimeSpan(23,59,0)
                });
                return result;
            }

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

        public async Task<List<PlaygroundShortViewModel>> GetAllShortModelsAsync(PlaygroundQueryModel queryModel)
        {
            string search = queryModel.Search?.ToUpper();
            List<Playground> playgrounds = await _commonRepository.FindByCondition<Playground>(x => x.IsApproved)
                
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                //.Select(x => new Playground
                //{
                //    Id = x.Id,
                //    Latitude = x.Latitude,
                //    Longitude = x.Longitude,
                //    IsApproved = x.IsApproved,
                //    IsCommercial = x.IsCommercial,
                //    CoveringType = x.CoveringType,
                //    City = x.City,
                //    Street = x.Street,
                //    HouseNumber = x.HouseNumber,
                //    OpenTime = x.OpenTime,
                //    CloseTime = x.CloseTime,
                //    Photo = x.Photo,
                //    PriceForOneHour = x.PriceForOneHour,
                //    Sports = x.Sports.Select(y => new PlaygroundSportConnection
                //    {
                //        Sport = new Sport
                //        {
                //            Name = y.Sport.Name,
                //            Id = y.Sport.Id
                //        }
                //    }).ToList()
                //})
                .Where(x => 
                    (string.IsNullOrEmpty(queryModel.Search) || x.Name.ToUpper().Contains(search) || x.City.ToUpper().Contains(search) || x.Street.ToUpper().Contains(search)) &&
                    (queryModel.IsCommercial == null || x.IsCommercial == queryModel.IsCommercial) &&
                    (string.IsNullOrEmpty(queryModel.Sport) || x.Sports.Any(x => x.Sport.Name == queryModel.Sport)) &&
                    (queryModel.TypeOfCovering == null || x.CoveringType == (byte)queryModel.TypeOfCovering))
                .Skip(queryModel?.Skip ?? 0).Take(queryModel?.Take ?? 20)
                .ToListAsync();

            return playgrounds.MapTo<List<PlaygroundShortViewModel>>();
        }

        public async Task UpdatePhotoAsync(byte[] fileBytes, long playgroundId) 
        {
            Playground entity = await _commonRepository.FindByIdAsync<Playground>(playgroundId);
            entity.Photo = fileBytes;
            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();
        }

        public async Task<List<PlaygroundMapViewModel>> GetPlaygroundsForMapAsync()
        {
            List<Playground> entities = await _commonRepository.FindByCondition<Playground>(x => x.IsApproved == true)
                .Select(x => new Playground
                {
                    Name = x.Name,
                    Id = x.Id,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                })
                .ToListAsync();

            return entities.MapTo<List<PlaygroundMapViewModel>>();
        }
    }
}
