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
    public class RentService : IRentService
    {
        private readonly ICommonRepository _commonRepository;

        public RentService(ICommonRepository commonRepository) 
        {
            _commonRepository = commonRepository;
        }

        public async Task<RentRequestViewModel> AddRentRequestAsync(string userId, RentRquestDtoModel model) 
        {
            var entity = model.MapTo<RentRequest>();

            entity.RenterId = userId;
            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task DeleteAsync(long id)
        {
            await _commonRepository.DeleteAsync<RentRequest>(id);
            await _commonRepository.SaveAsync();
        }

        public async Task<List<RentRequestViewModel>> GetAllAsync(string userId, long? playgroundId = null)
        {
            IQueryable<RentRequest> query = _commonRepository.GetAll<RentRequest>()
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .Include(x => x.Renter)
                .Include(x => x.Playground).ThenInclude(x => x.ResponsiblePerson);
            if (playgroundId != null)
                query = query.Where(x => x.PlaygroundId == playgroundId);
            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.Playground.ApplicationUserId == userId);

                var entities = await query.ToListAsync();

            return entities.MapTo<List<RentRequestViewModel>>();
        }

        public async Task<RentRequestViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<RentRequest>(x => x.Id == id)
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .Include(x => x.Renter)
                .Include(x => x.Playground).ThenInclude(x => x.ResponsiblePerson)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<RentRequestViewModel>();
        }

        public async Task<List<RentRequestViewModel>> ApproveRentRequestAsync(long rentRequestId, string userId, DateTime currentDate)
        {
            RentRequest rentRequest = await _commonRepository.FindByCondition<RentRequest>(x => x.Id == rentRequestId)
                .Include(x => x.Playground)
                .FirstOrDefaultAsync();

            if (rentRequest == null) return null;

            List<ConfirmedRent> rentsToAdd = await GenerateRentListAsync(rentRequest, currentDate);

            await _commonRepository.AddRangeAsync(rentsToAdd);
            await _commonRepository.SaveAsync();
            await DeleteAsync(rentRequestId);

            return await GetAllAsync(userId);
        }

        private async Task<List<ConfirmedRent>> GenerateRentListAsync(RentRequest request, DateTime currentDate) 
        {
            List<ConfirmedRent> rents = new List<ConfirmedRent>();
            if (request.IsOnce)
            {
                ConfirmedRent rent = FillRentFromRequest(request);
                rents.Add(rent);
                return rents;
            }

            DateTime startDate;
            DayOfWeek currentDay = currentDate.DayOfWeek;
            if ((byte)currentDay > request.DayOfTheWeek)
                startDate = currentDate.AddDays(7 + (request.DayOfTheWeek.Value - (byte)currentDay));
            else if ((byte)currentDay < request.DayOfTheWeek)
                startDate = currentDate.AddDays(request.DayOfTheWeek.Value - (byte)currentDay);
            else if(currentDate.AddHours(6).TimeOfDay >= request.StartTime)
                startDate = currentDate.AddDays(7 + (request.DayOfTheWeek.Value - (byte)currentDay));
            else
                startDate = currentDate;

            DateTime DateOfRent = startDate;
            while (DateOfRent < request.Date.Value) 
            {
                ConfirmedRent rent = FillRentFromRequest(request, DateOfRent);
                rents.Add(rent);
                DateOfRent = DateOfRent.AddDays(7);
            }

            if (await CheckGeneratedRentsConflictsAsync(rents, request.PlaygroundId, currentDate))
                return rents;
            return null;
        }

        private async Task<bool> CheckGeneratedRentsConflictsAsync(List<ConfirmedRent> rents, long playgroundId, DateTime currentDate) 
        {
            Playground playground = await _commonRepository.FindByIdAsync<Playground>(playgroundId);
            if (rents.Any(x => x.StartTime < playground.OpenTime || x.EndTime > playground.CloseTime))
                return false;

            List<ConfirmedRent> currentRents = await _commonRepository.FindByCondition<ConfirmedRent>(x => x.PlaygroundId == playgroundId && x.Date >= currentDate)
                .ToListAsync();

            foreach (ConfirmedRent rent in rents) 
                if (currentRents.Any(x => x.Date == rent.Date &&
                        (x.StartTime >= rent.StartTime && x.StartTime <= rent.EndTime ||
                        x.EndTime >= rent.StartTime && x.EndTime <= rent.EndTime)))
                    return false;
            return true;
        }

        private ConfirmedRent FillRentFromRequest(RentRequest request, DateTime? date = null) 
        {
            return new ConfirmedRent
            {
                IsOnce = request.IsOnce,
                PlaygroundId = request.PlaygroundId,
                Date = date ?? request.Date.Value,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                RenterId = request.RenterId,
                RenterName = request.Renter.FirstName + request.Renter.LastName,
                IsExecuted = false,
                Fee = request.Playground.PriceForOneHour.GetValueOrDefault() * (float)((request.EndTime.TotalMinutes - request.StartTime.TotalMinutes) / 60)
            };
        }

        public async Task<List<RentViewModel>> GetAllRentsAsync(long playgroundId, DateTime? date = null, DateTime? startDate = null, DateTime? endDate = null) 
        {
            IQueryable<ConfirmedRent> query = _commonRepository.FindByCondition<ConfirmedRent>(x => x.PlaygroundId == playgroundId)
               .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
               .Include(x => x.Renter).ThenInclude(x => x.ContactInformation)
               .Include(x => x.Playground).ThenInclude(x => x.ResponsiblePerson).ThenInclude(x => x.ContactInformation);

            query = query.Where(x => startDate != null && endDate != null && x.Date >= startDate.Value && x.Date <= endDate.Value);
            query = query.Where(x => date != null && x.Date == date.Value);

            List<ConfirmedRent> entities = await query.ToListAsync();
            return entities.MapTo<List<RentViewModel>>();
        }
    }
}
