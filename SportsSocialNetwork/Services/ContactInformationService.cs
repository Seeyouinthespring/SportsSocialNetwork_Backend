using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class ContactInformationService : IContactInformationService
    {
        private readonly ICommonRepository _commonRepository;

        public ContactInformationService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<ContactInformationViewModel> GetAsync(long id) 
        {
            var entity = await _commonRepository.FindByCondition<ContactInformation>(x => x.Id == id)
                .Include(x => x.Playground)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<ContactInformationViewModel>();
        }

        public async Task<ContactInformationViewModel> CreateAsync(ContactInformationDtoModel model)
        {
            var entity = model.MapTo<ContactInformation>();

            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task<ContactInformationViewModel> UpdateAsync(ContactInformationDtoModel model, long id)
        {
            var entity = await _commonRepository.FindByIdAsync<ContactInformation>(id);
            if (entity == null) return null;

            entity = Mapper.Map(model, entity);

            _commonRepository.Update(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public async Task DeleteAsync(long id) 
        {
            await _commonRepository.DeleteAsync<ContactInformation>(id);
            await _commonRepository.SaveAsync();
        }
    }
}
