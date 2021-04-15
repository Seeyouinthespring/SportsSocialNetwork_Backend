using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class ContactInformation : BaseEntity
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Playground))]
        public long? PlaygroundId { get; set; }
        public Playground Playground {get; set;}

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Vk { get; set; }

        public string Instagram { get; set; }
    }
}
