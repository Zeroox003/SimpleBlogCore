using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SimpleBlogCore.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string AboutMe { get; set; }

        public DateTime RegistrationDate { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public User()
        { }
    }
}
