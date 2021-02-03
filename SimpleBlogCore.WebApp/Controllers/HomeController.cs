using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleBlogCore.Domain.Interfaces;
using SimpleBlogCore.WebApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SimpleBlogCore.WebApp.Controllers
{
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostAsyncRepository repository;

        public HomeController(ILogger<HomeController> logger, IPostAsyncRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pagedListViewModel = await PostsPagedListViewModel.BuildDefaultModel(repository, page ?? 1);
            ViewBag.Title = "Latest posts";
            return View(pagedListViewModel);
        }

        public async Task<IActionResult> GetByTag(string tagName, int? page) 
        {
            var pagedListViewModel = await PostsPagedListViewModel.BuildModelForTag(repository, page ?? 1, tagName);
            ViewBag.Title = $"Latest posts tagged in «{tagName}»";
            return View("Index", pagedListViewModel);
        }

        public async Task<IActionResult> GetBySearch(string search, int? page)
        {
            var pagedListViewModel = await PostsPagedListViewModel.BuildModelForSearch(repository, page ?? 1, search);
            ViewBag.Title = $"Latest posts with the searched word «{search}»";
            return View("Index", pagedListViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Aboutme()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }
    }
}
