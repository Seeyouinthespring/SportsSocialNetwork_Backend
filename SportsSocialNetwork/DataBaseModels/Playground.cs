using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class Playground : BaseEntity
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Name { get; set; }

        public byte CoveringType { get; set; }

        public bool Lightning { get; set; }

        public byte TypeOfBuilding { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public TimeSpan? OpenTime { get; set; }

        public TimeSpan? CloseTime { get; set; }

        public DateTime? ClosedTill { get; set; }

        public byte[] Photo { get; set; }

        public float? Square { get; set; }

        public bool IsCommercial { get; set; } 

        public int? PriceForOneHour { get; set; }

        public bool IsApproved { get; set; }

        [ForeignKey(nameof(ResponsiblePerson))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ResponsiblePerson { get; set; }

        public ContactInformation ContactInformation { get; set; }
        public ICollection<RentRequest> RentRequests { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<PlaygroundSportConnection> Sports { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
