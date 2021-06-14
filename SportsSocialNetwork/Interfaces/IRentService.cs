using SportsSocialNetwork.Business.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IRentService
    {
        Task<RentRequestViewModel> AddRentRequestAsync(string userId, RentRquestDtoModel model);

        Task<List<RentRequestViewModel>> GetAllAsync(string userId = null, long? playgroundId = null);

        Task<List<RentRequestShortViewModel>> GetForSportsmanAsync(string userId, DateTime date);

        Task<List<RentShortViewModel>> GetRentsForSportsmanAsync(string userId, DateTime currentDate, DateTime? date = null, bool isFuture = true);

        Task<RentRequestFullViewModel> GetRequestAsync(long id, string currentUserID, DateTime currentDate, string role);

        Task<RentRequestViewModel> GetAsync(long id);

        Task DeleteAsync(long id);

        Task<List<RentViewModel>> GetAllRentsAsync(long playgroundId, DateTime? date = null, DateTime? startDate = null, DateTime? endDate = null);

        Task<List<RentRequestViewModel>> ApproveRentRequestAsync(long rentRequestId, string userId, DateTime currentDate);
    }
}
