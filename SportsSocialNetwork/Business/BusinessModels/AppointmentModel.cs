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

    public class AppointmentShortViewModel : AppointmentDtoModel 
    { 
        public long Id { get; set; }
        public string InitiatorFirstName { get; set; }
        public string InitiatorLastName { get; set; }
        public string PlaygroundName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Sport { get; set; }
        public int CurrentVisitors { get; set; }
    }
}
