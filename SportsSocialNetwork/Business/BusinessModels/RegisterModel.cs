using System;
using System.ComponentModel.DataAnnotations;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public bool Gender { get; set; }
    }
}
