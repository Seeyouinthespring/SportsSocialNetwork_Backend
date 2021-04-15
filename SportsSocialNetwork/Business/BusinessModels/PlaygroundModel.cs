using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static SportsSocialNetwork.Business.Enums.Enums;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class PlaygroundDtoModel
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public TypeOfCovering? CoveringType { get; set; }

        [Required]
        public bool? Lightning { get; set; }

        [Required]
        public TypeOfBuilding? TypeOfBuilding { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public TimeSpan? OpenTime { get; set; }

        public TimeSpan? CloseTime { get; set; }

        public DateTime? ClosedTill { get; set; }

        public string Photo { get; set; }

        public float? Square { get; set; }

        //public string ApplicationUserId { get; set; }
        
        [Required]
        public long[] SportsIds { get; set; }
    }

    public class PlaygroundViewModel : PlaygroundDtoModel
    {
        public long Id { get; set; }

        //public ResponsiblePersonViewModel ResponsiblePerson { get; set; }

        public ICollection<SportViewModel> SportsProvided { get; set; } 
    }
}
