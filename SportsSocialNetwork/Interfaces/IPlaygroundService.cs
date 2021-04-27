using SportsSocialNetwork.Business.BusinessModels;
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
    }
}
