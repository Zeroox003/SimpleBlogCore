using SimpleBlogCore.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlogCore.WebApp.Models.Account
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        [Display(Name = "About me")]
        public string AboutMe { get; set; }

        [Display(Name = "Registration date")]
        public DateTime RegistrationDate { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Email = user.Email;
            UserName = user.UserName;
            Gender = user.Gender;
            Address = user.Address;
            AboutMe = user.AboutMe;
            RegistrationDate = user.RegistrationDate;
        }

        public void UpdatePersonalData(User user)
        {
            user.UserName = UserName;
            user.Gender = Gender;
            user.Address = Address;
            user.AboutMe = AboutMe;
        }
    }
}
