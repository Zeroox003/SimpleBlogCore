using Microsoft.AspNetCore.Identity;
using SimpleBlogCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogCore.WebApp.Areas.Admin.Models
{
	public class UserViewModel
	{
		public Guid Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string Role { get; set; }

		public UserViewModel()
		{

		}

		public UserViewModel(User user, IEnumerable<string> userRoles)
		{
			Id = user.Id;
			Username = user.UserName;
			Email = user.Email;
			Role = userRoles.Single();
		}
	}
}
