using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using System;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IUserService
    {
        ApplicationUser GetUserByNameAsync(string name);
        Task<string> GetRoleByNameAsync(string name);
        Task UpdatePhotoAsync(PhotoModel model, string userId);
        Task UpdatePhotoAsync(byte[] bytes, string userId);
        Task<ProfileViewModel> GetProfileAsync(string userId, DateTime currentDate);
    }
}
