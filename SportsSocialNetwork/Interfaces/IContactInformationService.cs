using SportsSocialNetwork.Business.BusinessModels;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IContactInformationService
    {
        Task<ContactInformationViewModel> GetAsync(long id);
        Task<ContactInformationViewModel> CreateAsync(ContactInformationDtoModel model); 
        Task<ContactInformationViewModel> UpdateAsync(ContactInformationDtoModel model, long id); 
        Task DeleteAsync(long id);
    }
}
