using SportsSocialNetwork.Business.BusinessModels;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IVisitingsService
    {
        Task<VisitingBaseModel> GetVisitingByIdAsync(long id);
        Task<VisitingBaseModel> GetVisitingByAppointmentAsync(long appointmentId, string userId);
        Task<VisitingBaseModel> UpdateStatusAsync(long id, string currentUserId);
        Task<VisitingBaseModel> CreateAsync(long appointmentId, string userId);
        Task DeleteAsync(long id, string currentUserId);
    }
}
