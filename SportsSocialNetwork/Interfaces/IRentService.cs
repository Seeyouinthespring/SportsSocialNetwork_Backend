using SportsSocialNetwork.Business.BusinessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IRentService
    {
        Task<RentRequestViewModel> AddRentRequestAsync(string userId, RentRquestDtoModel model);

        Task<List<RentRequestViewModel>> GetAllAsync();

        Task<RentRequestViewModel> GetAsync(long id);

        Task DeleteAsync(long id);
    }
}
