using SportsSocialNetwork.Business.Enums;
using System.Collections.Generic;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class ApplicationUserMessageViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
    }

    public class ApplicationUserBaseViewModel
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public string Photo { get; set; }
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

    public class PhotoModel
    {
        public string Photo { get; set; }
    }

    public class ProfileViewModel : ApplicationUserBaseViewModel
    {
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Vk { get; set; }

        public string Instagram { get; set; }

        public List<CommonActivityViewModel> LatestActivities { get; set; }

        public List<CommonActivityViewModel> NearestActivities { get; set; }
    }
}
