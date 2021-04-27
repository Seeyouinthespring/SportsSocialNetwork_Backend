using SportsSocialNetwork.Attributes;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class ContactInformationDtoModel
    {
        [RequiredIfOtherIsNull(nameof(PlaygroundId))]
        public string ApplicationUserId { get; set; }

        [RequiredIfOtherIsNull(nameof(ApplicationUserId))]
        public long? PlaygroundId { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Vk { get; set; }

        public string Instagram { get; set; }
    }

    public class ContactInformationViewModel : ContactInformationDtoModel
    {
        public long Id { get; set; }

        public PlaygroundViewModel Playground { get; set; }
    }
}
