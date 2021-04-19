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

        [Required(ErrorMessage = "FirstNAme is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public bool Gender { get; set; }
    }
}
