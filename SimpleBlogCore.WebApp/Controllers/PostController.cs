using Microsoft.AspNetCore.Mvc;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using SimpleBlogCore.WebApp.Models;
using System;
using System.Threading.Tasks;

namespace SimpleBlogCore.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostAsyncRepository repository;

        public PostController(IPostAsyncRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ActionResult> Get(Guid id)
        {
            var post = await repository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View("Post", new PostViewModel(post));
        }
    }
}
