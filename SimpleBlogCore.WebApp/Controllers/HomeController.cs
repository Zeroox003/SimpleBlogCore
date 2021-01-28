using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleBlogCore.DataProvider;
using SimpleBlogCore.DataProvider.Repositories;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using SimpleBlogCore.WebApp.Models;
using System.Diagnostics;
using System.Linq;
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

        public async Task<IActionResult> Index()
        {
            var posts = (await repository.GetAll()).Select(post => new PostPreviewViewModel(post));
            return View(posts);
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
