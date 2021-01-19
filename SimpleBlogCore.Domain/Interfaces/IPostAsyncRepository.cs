using SimpleBlogCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogCore.Domain.Interfaces
{
    public interface IPostAsyncRepository : IAsyncRepository<Post>
    {
        /// <summary>
        /// Get all posts
        /// </summary>
        /// <param name="isPublished">If set true, then get only published posts</param>
        /// <returns>All or only published posts</returns>
        public Task<IReadOnlyList<Post>> GetAll(bool onlyPublished = true);
    }
}
