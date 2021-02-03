using Microsoft.AspNetCore.Mvc;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using SimpleBlogCore.WebApp.Models;

namespace SimpleBlogCore.WebApp.ViewComponents
{
	public class SidebarViewComponent : ViewComponent
	{
		private readonly IPostAsyncRepository _postRepository;
		private readonly IAsyncRepository<Tag> _tagRepository;

		public SidebarViewComponent(IPostAsyncRepository postRepository, IAsyncRepository<Tag> tagRepository)
		{
			_postRepository = postRepository;
			_tagRepository = tagRepository;
		}

		public IViewComponentResult Invoke()
		{
			var sidebarViewModel = new SidebarViewModel(_postRepository, _tagRepository);
			return View(sidebarViewModel);
		}
	}
}
