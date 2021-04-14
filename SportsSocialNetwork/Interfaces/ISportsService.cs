using SportsSocialNetwork.Business.BusinessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface ISportsService
    {
        Task<SportViewModel> CreateAsync(SportDtoModel model);
        Task<SportViewModel> UpdateAsync(SportDtoModel model, long id);
        Task<List<SportViewModel>> GetAllAsync(string search = null);
        Task<SportViewModel> GetAsync(long id);
        Task DeleteAsync(long id);
    }
}
