using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class AppointmentVisiting : BaseEntity
    {
        [ForeignKey(nameof(Member))]
        public string MemberId { get; set;}
        public ApplicationUser Member { get; set; }

        [ForeignKey(nameof(Appointment))]
        public long AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        
        public byte Status { get; set; }
    }
}
