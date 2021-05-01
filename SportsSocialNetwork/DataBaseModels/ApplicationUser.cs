using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SportsSocialNetwork.DataBaseModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }

        public byte[] Photo { get; set; }

        public virtual ICollection<Message> ReceivedMessages { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public ICollection<RentRequest> RentRequests { get; set; }
        public ICollection<ConfirmedRent> ConfirmedRents { get; set; }
        public ICollection<AppointmentVisiting> VisitedAppointments { get; set; }
    }
}
