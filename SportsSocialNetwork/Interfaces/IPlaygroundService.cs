using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Business.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IPlaygroundService
    {
        Task<PlaygroundViewModel> CreateAsync(PlaygroundDtoModel model, string userId);
        Task<PlaygroundViewModel> UpdateAsync(PlaygroundDtoModel model, long id, string userId);
        Task<List<PlaygroundViewModel>> GetAllAsync(string search = null);
        Task<PlaygroundViewModel> GetAsync(long id);
        Task DeleteAsync(long id);
        
        Task<PlaygroundViewModel> ApproveAsync(long id);
        Task<PlaygroundSummaryInfoViewModel> GetSummaryInfoAsync(long id);
        Task<List<TimingIntervalModel>> GetFreeTimingsAsync(long id, DateTime date);
        Task<VisitorsNumberViewModel> GetVisitorsNumberAsync(long id, DateTime date, TimeSpan time);

        Task<List<PlaygroundShortViewModel>> GetAllShortModelsAsync(PlaygroundQueryModel queryModel);

        Task UpdatePhotoAsync(byte[] fileBytes, long playgroundId);
    }

    public class PlaygroundQueryModel : SkipTakeRequestParamsModel
    {
        public bool? IsCommercial { get; set; }

        public string Sport { get; set; }

        public TypeOfCovering? TypeOfCovering {get; set;}

        public string Search { get; set; }
    }
}
