using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class PersonalActivity : BaseDateEntity
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [ForeignKey(nameof(Initiator))]
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }

        [ForeignKey(nameof(Playground))]
        public long PlaygroundId { get; set; }
        public Playground Playground { get; set; }

        public bool IsVisited { get; set; }
    }
}
