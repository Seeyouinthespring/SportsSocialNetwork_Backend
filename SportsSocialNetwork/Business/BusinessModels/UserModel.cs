using SportsSocialNetwork.Business.Enums;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class ApplicationUserBaseViewModel
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
    }

    public class ApplicationUserVisitingViewModel : ApplicationUserBaseViewModel
    {
        public long VisitingId { get; set; }
        public VisitingStatus Status { get; set; }
    }

    public class ApplicationUserRenterViewModel : ApplicationUserBaseViewModel
    {
        public long RentRequestId { get; set; }
    }

    public class ApplicationUserLandlordViewModel : ApplicationUserBaseViewModel 
    {
        
    }
}
