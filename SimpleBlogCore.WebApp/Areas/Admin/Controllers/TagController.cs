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
	public class TagController : Controller
	{
		private readonly IAsyncRepository<Tag> tagRepository;

		public TagController(IAsyncRepository<Tag> tagRepository)
		{
			this.tagRepository = tagRepository;
		}

		public async Task<IActionResult> GetAll()
		{
			var tags = (await tagRepository.GetAll()).Select(t => new TagViewModel(t));

			return Json(new { data = tags.ToArray() }, new JsonSerializerOptions {
				WriteIndented = true,
			});
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid tagId)
		{
			var post = await tagRepository.GetById(tagId);
			await tagRepository.Remove(post);
			return Ok();
		}

		public IActionResult Add()
		{
			return View(new TagViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(TagViewModel tagViewModel)
		{
			var tag = tagViewModel.GetEntity();
			await tagRepository.Add(tag);

			return RedirectToAction("Index", "Home", new { Area = "Admin" });
		}

		public async Task<IActionResult> Edit(Guid id)
		{
			var tag = await tagRepository.GetById(id);
			return View(new TagViewModel(tag));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(TagViewModel tagViewModel)
		{
			var tag = await tagRepository.GetById(tagViewModel.Id.Value);
			tagViewModel.Update(tag);
			await tagRepository.Update(tag);

			return RedirectToAction("Index", "Home", new { Area = "Admin" });
		}
	}
}
