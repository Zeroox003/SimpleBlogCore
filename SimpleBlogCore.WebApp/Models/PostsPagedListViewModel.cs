using SimpleBlogCore.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SimpleBlogCore.WebApp.Models
{
	public class PostsPagedListViewModel
	{
		public IPagedList<PostPreviewViewModel> Posts { get; private set; }

		public const int PostsCountOnPage = 10;

		public PostsPagedListViewModel()
		{
		}

		public static async Task<PostsPagedListViewModel> BuildDefaultModel(IPostAsyncRepository postRepository, int pageNum)
		{
			return new PostsPagedListViewModel() {
				Posts = (await postRepository.GetAll()
					.ToPagedListAsync(pageNum, PostsCountOnPage))
					.Select(p => new PostPreviewViewModel(p))
			};
		}

		public static async Task<PostsPagedListViewModel> BuildModelForTag(IPostAsyncRepository postRepository, int pageNum, string tagName)
		{
			return new PostsPagedListViewModel() {
				Posts = (await postRepository.GetForTag(tagName)
					.ToPagedListAsync(pageNum, PostsCountOnPage))
					.Select(p => new PostPreviewViewModel(p))
			};
		}

		public static async Task<PostsPagedListViewModel> BuildModelForSearch(IPostAsyncRepository postRepository, int pageNum, string search)
		{
			return new PostsPagedListViewModel() {
				Posts = (await postRepository.GetForSearch(search)
					.ToPagedListAsync(pageNum, PostsCountOnPage))
					.Select(p => new PostPreviewViewModel(p))
			};
		}
	}
}
