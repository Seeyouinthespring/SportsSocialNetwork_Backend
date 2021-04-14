using Microsoft.AspNetCore.Identity;
using System;

namespace SportsSocialNetwork.DataBaseModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }
    }
}
