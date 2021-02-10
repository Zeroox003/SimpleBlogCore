using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using SimpleBlogCore.WebApp.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleBlogCore.WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class PostController : Controller
	{
		private readonly IPostAsyncRepository postRepository;
		private readonly IAsyncRepository<Tag> tagRepository;

		public PostController(IPostAsyncRepository postRepository, IAsyncRepository<Tag> tagRepository)
		{
			this.postRepository = postRepository;
			this.tagRepository = tagRepository;
		}

		public async Task<IActionResult> GetAll()
		{
			var posts = (await postRepository.GetAllAsync(false)).Select(p => new PostViewModel(p));

			return Json(new { data = posts.ToArray() }, new JsonSerializerOptions {
				WriteIndented = true,
			});
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid postId)
		{
			var post = await postRepository.GetById(postId);
			await postRepository.Remove(post);
			return Ok();
		}

		public async Task<IActionResult> PublishToggle(Guid postId)
		{
			var post = await postRepository.GetById(postId);
			post.IsPublished = !post.IsPublished;
			await postRepository.Update(post);
			return Ok();
		}

		public async Task<IActionResult> Add()
		{
			var tags = await tagRepository.GetAll();
			return View(new PostEditViewModel(tags));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(PostEditViewModel postViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(postViewModel);
			}

			var post = await postViewModel.GetEntity(tagRepository);
			await postRepository.Add(post);

			return RedirectToAction("Get", "Post", new { Area = "", id = post.Id });
		}

		public async Task<IActionResult> Edit(Guid id)
		{
			var post = await postRepository.GetById(id);
			var tags = await tagRepository.GetAll();
			return View(new PostEditViewModel(post, tags));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(PostEditViewModel postViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(postViewModel);
			}

			var post = await postRepository.GetById(postViewModel.Id.Value);
			await postViewModel.Update(post, tagRepository);
			await postRepository.Update(post);

			return RedirectToAction("Get", "Post", new { Area = "", id = post.Id });
		}
	}
}
