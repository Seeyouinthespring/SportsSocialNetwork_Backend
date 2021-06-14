using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class RentViewModel
    {
        public DateTime Date { get; set; }

        public long PlaygroundId { get; set; }

        public PlaygroundViewModel Playground { get; set; }

        public string RenterId { get; set; }

        public ApplicationUserRenterViewModel Renter { get; set; }

        public string RenterName { get; set; }

        public bool IsOnce { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsExecuted { get; set; }

        public float Fee { get; set; }
    }

    public class RentShortViewModel
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public string PlaygroundName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsExecuted { get; set; }

        public float Fee { get; set; }
    }
}
