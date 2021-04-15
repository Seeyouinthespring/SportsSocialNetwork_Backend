using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class Message : BaseDateEntity
    {
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }
        [InverseProperty(nameof(ApplicationUser.SentMessages))]
        public virtual ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(Receiver))]
        public string ReceiverId { get; set; }
        [InverseProperty(nameof(ApplicationUser.ReceivedMessages))]
        public virtual ApplicationUser Receiver { get; set; }

        public string Text { get; set; }
    }
}
