using SportsSocialNetwork.Business.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface ICommentsService
    {
        Task<List<CommentViewModel>> GetAllAsync(long playgroundId, int skip = 0, int take = 20);
        Task<CommentViewModel> GetAsync(long id);
        Task<List<CommentViewModel>> CreateAsync(CommentDtoModel model, string userId, long playgroundId, DateTime currentDate);
        Task<List<CommentViewModel>> UpdateAsync(CommentDtoModel model, long id);
        Task DeleteAsync(long id);
    }
}
