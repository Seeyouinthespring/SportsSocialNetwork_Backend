using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class PlaygroundService : IPlaygroundService
    {
        private readonly ICommonRepository _commonRepository;

        public PlaygroundService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public virtual async Task<PlaygroundViewModel> CreateAsync(PlaygroundDtoModel model)
        {
            var entity = model.MapTo<Playground>();

            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<PlaygroundViewModel> UpdateAsync(PlaygroundDtoModel model, long id)
        {
            Playground entity = await _commonRepository.FindByCondition<Playground>(x => x.Id == id)
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            entity = Mapper.Map(model, entity);
            //entity = model.MapTo<Playground>();

             _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<List<PlaygroundViewModel>> GetAllAsync(string search = null)
        {
            List<Playground> entities = await _commonRepository.GetAll<Playground>()
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .ToListAsync();

            return entities.MapTo<List<PlaygroundViewModel>>();
        }

        public async Task<PlaygroundViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<Playground>(x => x.Id == id)
                .Include(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<PlaygroundViewModel>();
        }

        public async Task DeleteAsync(long id)
        {
            await _commonRepository.DeleteAsync<Playground>(id);

            await _commonRepository.SaveAsync();
        }
    }
}
