using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class UserService : IUserService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ICommonRepository commonRepository, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _commonRepository = commonRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> GetRoleByNameAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.First();
            //string role = _roleManager.GetRoleNameAsync();

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
