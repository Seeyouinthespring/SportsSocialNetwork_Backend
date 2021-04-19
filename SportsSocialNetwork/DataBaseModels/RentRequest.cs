using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class RentRequest : BaseEntity
    {
        [ForeignKey(nameof(Playground))]
        public long PlaygroundId { get; set; }
        public Playground Playground { get; set; }

        [ForeignKey(nameof(Renter))]
        public string RenterId { get; set; }
        public ApplicationUser Renter { get; set; }

        public bool IsOnce { get; set; }

        public int DurationDays {get; set;}

        public DateTime? Date { get; set; }
        
        public byte DayOfTheWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Description { get; set; }
    }
}
