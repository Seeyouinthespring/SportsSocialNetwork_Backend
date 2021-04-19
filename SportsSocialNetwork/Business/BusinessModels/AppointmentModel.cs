using System;
using System.Collections.Generic;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class AppointmentDtoModel
    {
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public long PlaygroundId { get; set; }
        
        public int ParticipantsQuantity { get; set; }

        public string Description { get; set; }
    }

    public class AppointmentViewModel : AppointmentDtoModel
    {
        public long Id { get; set; }

        public PlaygroundViewModel Playground {get; set;}

        public List<ApplicationUserVisitingViewModel> PlannedVisits { get; set; }

        public ApplicationUserBaseViewModel Initiator { get; set; }
    }
}
