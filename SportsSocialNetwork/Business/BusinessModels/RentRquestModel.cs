using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class RentRquestDtoModel
    {
        [Required]
        public long? PlaygroundId { get; set; }

        [Required]
        public bool? IsOnce { get; set; }

        public int? DurationDays { get; set; }

        public DateTime? Date { get; set; }

        public byte DayOfTheWeek { get; set; }

        [Required]
        public TimeSpan? StartTime { get; set; }

        [Required]
        public TimeSpan? EndTime { get; set; }

        public string Description { get; set; }
    }

    public class RentRequestViewModel : RentRquestDtoModel 
    {
        public long Id { get; set; }

        public PlaygroundViewModel Playground { get; set; }

        public ApplicationUserRenterViewModel Renter { get; set; }

        public ApplicationUserLandlordViewModel Landlord { get; set; }
    }
}
