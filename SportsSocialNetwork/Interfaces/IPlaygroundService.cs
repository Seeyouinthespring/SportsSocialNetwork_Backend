using SportsSocialNetwork.Business.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IPlaygroundService
    {
        Task<PlaygroundViewModel> CreateAsync(PlaygroundDtoModel model);
        Task<PlaygroundViewModel> UpdateAsync(PlaygroundDtoModel model, long id);
        Task<List<PlaygroundViewModel>> GetAllAsync(string search = null);
        Task<PlaygroundViewModel> GetAsync(long id);
        Task DeleteAsync(long id);
    }
}
