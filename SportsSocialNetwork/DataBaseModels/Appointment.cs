using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class Appointment : BaseDateEntity
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [ForeignKey(nameof(Initiator))]
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }

        [ForeignKey(nameof(Playground))]
        public long PlaygroundId { get; set; }
        public Playground Playground { get; set; }

        public int ParticipantsQuantity { get; set; }

        public string Description { get; set; }

        [ForeignKey(nameof(Sport))]
        public long SportId { get; set; }
        public Sport Sport { get; set; }

        public ICollection<AppointmentVisiting> Visits { get; set; }

        [NotMapped]
        public bool Participation { get; set; }
    }
}
