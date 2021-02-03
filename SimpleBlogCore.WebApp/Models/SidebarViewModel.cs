using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogCore.WebApp.Models
{
	public class SidebarViewModel
	{
		public IList<TagViewModel> Tags { get; set; }

		public IList<PostPreviewViewModel> LatestPosts { get; set; }

		public SidebarViewModel(IPostAsyncRepository postRepository, IAsyncRepository<Tag> tagRepository)
		{
			Task.Run(async () => {
				LatestPosts = postRepository.GetAll()
				.OrderByDescending(p => p.Created)
				.Take(15)
				.Select(p => new PostPreviewViewModel(p))
				.ToList();

				Tags = (await tagRepository.GetAll())
					.Select(t => new TagViewModel(t))
					.ToList();
			}).Wait();
		}
	}
}
