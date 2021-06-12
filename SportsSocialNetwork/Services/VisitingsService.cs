using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System.Threading.Tasks;
using SportsSocialNetwork.Business.Enums;
using Microsoft.EntityFrameworkCore;

namespace SportsSocialNetwork.Services
{
    public class VisitingsService : IVisitingsService
    {
        private readonly ICommonRepository _commonRepository;

        public VisitingsService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<VisitingBaseModel> GetVisitingByIdAsync(long id)
        {
            var entity = await _commonRepository.FindFirstByConditionAsync<AppointmentVisiting>(x => x.Id == id);

            if (entity == null) return null;

            return entity.MapTo<VisitingBaseModel>();
        }

        public async Task<VisitingBaseModel> GetVisitingByAppointmentAsync(long appointmentId, string userId)
        {
            var entity = await _commonRepository.FindByCondition<AppointmentVisiting>(x => x.AppointmentId == appointmentId && userId == x.MemberId)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<VisitingBaseModel>();
        }

        public async Task<VisitingBaseModel> CreateAsync(long appointmentId, string userId)
        {
            AppointmentVisiting entity = new AppointmentVisiting
            {
                AppointmentId = appointmentId,
                MemberId = userId,
                Status = (byte)VisitingStatus.PlanningToVisit
            };
            
            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetVisitingByIdAsync(entity.Id);
        }

        public async Task DeleteAsync(long id, string currentUserId)
        {
            AppointmentVisiting entity = await _commonRepository.FindFirstByConditionAsync<AppointmentVisiting>(x => x.AppointmentId == id && x.MemberId == currentUserId);

            await _commonRepository.DeleteAsync<AppointmentVisiting>(entity.Id);

            await _commonRepository.SaveAsync();
        }

        public async Task<VisitingBaseModel> UpdateStatusAsync(long id,string currentUserId)
        {
            AppointmentVisiting entity = await _commonRepository.FindFirstByConditionAsync<AppointmentVisiting>(x => x.AppointmentId == id && x.MemberId == currentUserId);

            if (entity == null) return null;

            if (entity.Status == 1) 
                entity.Status = 0; 
            else 
                entity.Status = 1;

            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetVisitingByIdAsync(entity.Id);

        }
    }
}
