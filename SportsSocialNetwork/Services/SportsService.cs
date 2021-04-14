using AutoMapper;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class SportsService : ISportsService
    {
        private readonly ICommonRepository _commonRepository;

        public SportsService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public virtual async Task<SportViewModel> CreateAsync(SportDtoModel model)
        {
            var entity = model.MapTo<Sport>();

            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<SportViewModel> UpdateAsync(SportDtoModel model, long id)
        {
            Sport entity = await _commonRepository.FindByIdAsync<Sport>(id);
            if (entity == null) return null;

            entity = Mapper.Map(model, entity);

            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<List<SportViewModel>> GetAllAsync(string search = null)
        {
            List<Sport> entities = await _commonRepository.GetAllAsync<Sport>();

            return entities.MapTo<List<SportViewModel>>();
        }

        public async Task<SportViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByIdAsync<Sport>(id);

            if (entity == null) return null;

            return entity.MapTo<SportViewModel>();
        }

        public async Task DeleteAsync(long id)
        {
            await _commonRepository.DeleteAsync<Sport>(id);

            await _commonRepository.SaveAsync();
        }
    }
}
