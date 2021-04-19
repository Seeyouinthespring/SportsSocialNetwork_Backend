using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Interfaces;
using System.Linq;

namespace SportsSocialNetwork.Services
{
    public class UserService : IUserService
    {
        private readonly ICommonRepository _commonRepository;

        public UserService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public ApplicationUser GetUserByNameAsync(string name)
        {
            var entity = _commonRepository.FindByCondition<ApplicationUser>(x => x.NormalizedUserName == name.ToUpper())
                .Include(x => x.ContactInformation)
                .FirstOrDefault();

            if (entity == null)
                return null;
            return entity;
        }
    }
}
