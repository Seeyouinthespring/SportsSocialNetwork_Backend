using System;
using System.ComponentModel.DataAnnotations;


namespace SportsSocialNetwork.Business.BusinessModels
{
    public class RentRquestDtoModel
    {
        [Required]
        public long PlaygroundId { get; set; }

        [Required]
        public bool IsOnce { get; set; }

        public DateTime? Date { get; set; }

        public byte? DayOfTheWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public string Description { get; set; }
    }

    public class RentRequestShortViewModel : RentRquestDtoModel
    {
        public long Id { get; set; }

        public string PlaygroundName { get; set; }
    }

    public class RentRequestFullViewModel : RentRequestShortViewModel 
    {
        public double PlaygroundLatitude { get; set; }
        public double PlaygroundLongitude { get; set; }

        public string PlaygroundCity { get; set; }
        public string PlaygroundHouseNumber { get; set; }
        public string PlaygroundStreet { get; set; }
        public string PlaygroundPhoto { get; set; }
        public string PersonPhoto { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }
        public int PersonAge { get; set; }
        public string PersonId { get; set; }
        public bool PersonGender { get; set; }

    }

    public class RentRequestViewModel : RentRquestDtoModel 
    {
        public long Id { get; set; }

        public PlaygroundViewModel Playground { get; set; }

        public ApplicationUserRenterViewModel Renter { get; set; }

        public ApplicationUserLandlordViewModel Landlord { get; set; }
    }
}
