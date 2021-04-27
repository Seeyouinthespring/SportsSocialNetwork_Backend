using AutoMapper;
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
    public class PersonalActivityService : IPersonalActivityService
    {
        private readonly ICommonRepository _commonRepository;

        public PersonalActivityService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<PersonalActivityViewModel> CreateAsync(PersonalActivityDtoModel model, string userId)
        {
            var entity = model.MapTo<PersonalActivity>();

            entity.InitiatorId = userId;
            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task DeleteAsync(long id)
        {
            await _commonRepository.DeleteAsync<PersonalActivity>(id);
            await _commonRepository.SaveAsync();
        }

        public async Task<List<PersonalActivityViewModel>> GetAllAsync(string userId)
        {
            List<PersonalActivity> entities = await _commonRepository.GetAll<PersonalActivity>()
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .ToListAsync();

            return entities.MapTo<List<PersonalActivityViewModel>>();
        }

        public async Task<PersonalActivityViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<PersonalActivity>(x => x.Id == id)
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<PersonalActivityViewModel>();
        }

        public async Task<PersonalActivityViewModel> UpdateAsync(PersonalActivityDtoModel model, long id, string userId)
        {
            var entity = await _commonRepository.FindByCondition<PersonalActivity>(x => x.Id == id)
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            entity = Mapper.Map(model, entity);
            entity.InitiatorId = userId;

            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }
    }
}
