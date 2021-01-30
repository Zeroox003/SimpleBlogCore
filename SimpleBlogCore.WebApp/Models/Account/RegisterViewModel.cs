using SimpleBlogCore.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SimpleBlogCore.WebApp.Models.Account
{
	public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        public RegisterViewModel()
        {

        }

        public User GetModel()
        {
            return new User {
                Id = Guid.NewGuid(),
                Email = Email,
                UserName = UserName,
                RegistrationDate = DateTime.UtcNow,
                ProfilePicturePath = Path.Combine("images", "placeholder.png"),
                EmailConfirmed = true,
                LockoutEnabled = false
            };
        }
    }
}
