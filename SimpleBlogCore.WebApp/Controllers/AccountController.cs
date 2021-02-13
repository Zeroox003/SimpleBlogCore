using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.WebApp.Models.Account;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JustBlog.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _env = env;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }                

            var model = new LoginViewModel {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Incorrect login or password");
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("", "A user with this Email already exists");
                return View(model);
            }

            user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                ModelState.AddModelError("", "A user with this Username already exists");
                return View(model);
            }

            var userModel = model.GetModel();
            var resultUserCreating = await userManager.CreateAsync(userModel, model.Password);
            if (!resultUserCreating.Succeeded)
            {
                foreach(var error in resultUserCreating.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            var resultAddingToRole = await userManager.AddToRoleAsync(userModel, role: "User");
            if (resultAddingToRole.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Something went wrong");
            return View(model);
        }

        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<ActionResult> UserProfile(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            return View(new UserViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserProfile(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            var user = await userManager.FindByIdAsync(userViewModel.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserName != userViewModel.UserName)
            {
                bool userNameIsFree = await userManager.FindByNameAsync(userViewModel.UserName) == null;
                if (!userNameIsFree)
                {
                    ModelState.AddModelError("", "A user with the same Username already exists");
                    return View(userViewModel);
                }
            }

            userViewModel.UpdatePersonalData(user);
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await signInManager.RefreshSignInAsync(user);
                return RedirectToAction("UserProfile", new { Id = user.Id });
            }

            ModelState.AddModelError("", "Something went wrong");
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadProfilePicture(IFormFile profileImage)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(profileImage.FileName);
            string relativePath = Path.Combine("Users", user.Id.ToString(), fileName);
            string fullPath = Path.Combine(_env.WebRootPath, relativePath);
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
			{
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			}

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
                user.ProfilePicturePath = $"~/{relativePath}";
            }

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await signInManager.RefreshSignInAsync(user);
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");
            }

            return RedirectToAction("UserProfile", new { Id = user.Id });
        }
    }
}
