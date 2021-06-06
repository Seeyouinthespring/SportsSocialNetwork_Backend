using SportsSocialNetwork.Business.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public enum AppointmentAdditingError 
    {
        WrongSport,
        WrongPlayground,
        WrongTimeInterval,
        DuplicateAppointment,
        Ok
    }

    public interface IAppointmentsService
    {
        Task<AppointmentAdditingError> CreateAsync(AppointmentDtoModel model, string userId);
        Task<SingleAppointmentViewModel> UpdateAsync(AppointmentDtoModel model, long id, string userId);
        Task<List<AppointmentShortViewModel>> GetAllAsync(bool? isActual, int? sportId, int? playgroundId, string userId, DateTime currentDate);
        Task<List<AppointmentShortViewModel>> GetForPlaygroundAsync(long id, DateTime currentDate, string currentUserId);
        Task<SingleAppointmentViewModel> GetAsync(long id);
        Task DeleteAsync(long id);
    }
}
