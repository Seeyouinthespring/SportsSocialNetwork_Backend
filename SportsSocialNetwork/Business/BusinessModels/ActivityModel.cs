using SportsSocialNetwork.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class CommonActivityViewModel
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string PlaygroundName {get; set;}

        public long PlaygroundId { get; set; }

        public bool IsExecuted { get; set; }

        public ActivityType Type { get; set; }
    }
}
