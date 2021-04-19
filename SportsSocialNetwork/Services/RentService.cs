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
    public class RentService : IRentService
    {
        private readonly ICommonRepository _commonRepository;

        public RentService(ICommonRepository commonRepository) 
        {
            _commonRepository = commonRepository;
        }

        public async Task<RentRequestViewModel> AddRentRequestAsync(string userId, RentRquestDtoModel model) 
        {
            var entity = model.MapTo<RentRequest>();

            entity.RenterId = userId;
            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            return await GetAsync(entity.Id);
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RentRequestViewModel>> GetAllAsync()
        {
            List<RentRequest> entities = await _commonRepository.GetAll<RentRequest>()
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .Include(x => x.Renter)
                .Include(x => x.Playground).ThenInclude(x => x.ResponsiblePerson)
                .ToListAsync();

            return entities.MapTo<List<RentRequestViewModel>>();
        }

        public async Task<RentRequestViewModel> GetAsync(long id)
        {
            var entity = await _commonRepository.FindByCondition<RentRequest>(x => x.Id == id)
                .Include(x => x.Playground).ThenInclude(x => x.Sports).ThenInclude(x => x.Sport)
                .Include(x => x.Renter)
                .Include(x => x.Playground).ThenInclude(x => x.ResponsiblePerson)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            return entity.MapTo<RentRequestViewModel>();
        }
    }
}
