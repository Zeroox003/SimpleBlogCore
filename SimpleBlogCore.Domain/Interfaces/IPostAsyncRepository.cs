using SimpleBlogCore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogCore.Domain.Interfaces
{
	public interface IPostAsyncRepository : IAsyncRepository<Post>
    {
        /// <summary>
        /// Get all posts async
        /// </summary>
        /// <param name="isPublished">If set true, then get only published posts</param>
        /// <returns>All or only published posts</returns>
        public Task<IReadOnlyList<Post>> GetAllAsync(bool onlyPublished = true);

        /// <summary>
        /// Get all posts
        /// </summary>
        /// <param name="isPublished">If set true, then get only published posts</param>
        /// <returns>All or only published posts</returns>
        public IQueryable<Post> GetAll(bool onlyPublished = true);

        /// <summary>
        /// Get collection of posts belongs to a particular tag
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Collection of posts belongs to a particular tag</returns>
        public IQueryable<Post> GetForTag(string tagName);

        /// <summary>
        /// Get collection of posts whose title contains the search term
        /// </summary>
        /// <param name="search">Search term</param>
        /// <returns>collection of posts whose title contains the search term</returns>
        public IQueryable<Post> GetForSearch(string search);
    }
}
