using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class ConfirmedRent : BaseDateEntity
    {
        [ForeignKey(nameof(Playground))]
        public long PlaygroundId { get; set; }
        public Playground Playground { get; set; }

        [ForeignKey(nameof(Renter))]
        public string RenterId { get; set; }
        public ApplicationUser Renter { get; set; }

        public string ApplicantName { get; set; }

        public bool IsOnce { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsExecuted { get; set; }

        public bool Fee { get; set; }
    }
}
