using SportsSocialNetwork.Business.BusinessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IAppointmentsService
    {
        Task<AppointmentViewModel> CreateAsync(AppointmentDtoModel model, string userId);
        Task<AppointmentViewModel> UpdateAsync(AppointmentDtoModel model, long id, string userId);
        Task<List<AppointmentViewModel>> GetAllAsync(string search = null);
        Task<AppointmentViewModel> GetAsync(long id);
        Task DeleteAsync(long id);
    }
}
