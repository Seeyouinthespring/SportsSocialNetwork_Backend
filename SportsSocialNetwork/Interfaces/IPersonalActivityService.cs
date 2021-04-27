using SportsSocialNetwork.Business.BusinessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IPersonalActivityService
    {
        Task<PersonalActivityViewModel> CreateAsync(PersonalActivityDtoModel model, string userId);
        Task<PersonalActivityViewModel> UpdateAsync(PersonalActivityDtoModel model, long id, string userId);
        Task<List<PersonalActivityViewModel>> GetAllAsync(string userId);
        Task<PersonalActivityViewModel> GetAsync(long id);
        Task DeleteAsync(long id);
    }
}
