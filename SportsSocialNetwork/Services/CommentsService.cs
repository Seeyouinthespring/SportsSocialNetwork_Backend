using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommonRepository _commonRepository;

        public CommentsService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<List<CommentViewModel>> GetAllAsync(long playgroundId, int skip, int take) 
        {
            var entities = await _commonRepository.FindByCondition<Comment>(x => x.PlaygroundId == playgroundId)
                .Include(x => x.Author).OrderByDescending(x => x.Date)
                .Skip(skip).Take(take).ToListAsync();
            return entities.MapTo<List<CommentViewModel>>();
        }

        public async Task<CommentViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<Comment>(x => x.Id == id)
                .Include(x => x.Author).FirstOrDefaultAsync();
            
            if (entity == null) return null;

            return entity.MapTo<CommentViewModel>();
        }

        public async Task<List<CommentViewModel>> CreateAsync(CommentDtoModel model, string userId, long playgroundId, DateTime currentDate) 
        {
            Comment entity = model.MapTo<Comment>();
            entity.PlaygroundId = playgroundId;
            entity.AuthorId = userId;
            entity.Date = currentDate;

            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAllAsync(playgroundId, 0, 20);
        }

        public async Task<List<CommentViewModel>> UpdateAsync(CommentDtoModel model, long id)
        {
            Comment entity = await _commonRepository.FindByIdAsync<Comment>(id);

            if (entity == null) return null;
            entity.Text = model.Text;

            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAllAsync(entity.PlaygroundId, 0, 20);
        }

        public async Task DeleteAsync(long id) 
        {
            await _commonRepository.DeleteAsync<Comment>(id);
            await _commonRepository.SaveAsync();
        }
    }
}
