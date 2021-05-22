using System;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class PersonalActivityDtoModel
    {
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public long PlaygroundId { get; set; }

        public string InitiatorId { get; set; }

        public bool IsVisited { get; set; }
    }

    public class PersonalActivityViewModel : PersonalActivityDtoModel
    {
        public long Id { get; set; }

        public PlaygroundViewModel Playground { get; set; }
    }
}
