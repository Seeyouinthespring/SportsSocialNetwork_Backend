using SportsSocialNetwork.DataBaseModels;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IUserService
    {
        ApplicationUser GetUserByNameAsync(string name);
        Task<string> GetRoleByNameAsync(string name);
    }
}
