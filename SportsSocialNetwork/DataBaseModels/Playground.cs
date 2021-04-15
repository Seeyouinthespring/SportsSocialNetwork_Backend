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

        public string Photo { get; set; }

        public float? Square { get; set; }

        [ForeignKey(nameof(ResponsiblePerson))]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ResponsiblePerson { get; set; }

        public ICollection<PlaygroundSportConnection> Sports { get; set; }
    }
}
