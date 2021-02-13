using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.WebApp.Areas.Admin.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleBlogCore.WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<User> userManager;

		public UserController(UserManager<User> userManager)
		{
			this.userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> GetAll()
		{
			var users = (await userManager.Users.ToListAsync())
				.Select(async u => new UserViewModel(u, await userManager.GetRolesAsync(u)))
				.Select(t => t.Result);

			return Json(new { data = users.ToArray() }, new JsonSerializerOptions {
				WriteIndented = true,
			});
		}

		[HttpPost]
		public async Task<IActionResult> RoleToggle(Guid userId)
		{
			var user = await userManager.FindByIdAsync(userId.ToString());
			if (await userManager.IsInRoleAsync(user, role: "Admin"))
			{
				await userManager.RemoveFromRoleAsync(user, role: "Admin");
				await userManager.AddToRoleAsync(user, role: "User");
			}
			else
			{
				await userManager.RemoveFromRoleAsync(user, role: "User");
				await userManager.AddToRoleAsync(user, role: "Admin");
			}

			return Ok();
		}
	}
}
