using Microsoft.EntityFrameworkCore;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogCore.DataProvider.Repositories
{
    public class PostAsyncRepository : EfAsyncBaseRepository<Post>, IPostAsyncRepository
    {
        public PostAsyncRepository(SimpleBlogDbContext context) : base(context)
        {

        }

        public async Task<IReadOnlyList<Post>> GetAllAsync(bool onlyPublished = true)
        {
            return await QueryAll.Where(p => !onlyPublished || p.IsPublished).OrderByDescending(p => p.Created).ToListAsync();
        }

        public IQueryable<Post> GetAll(bool onlyPublished = true)
		{
            return QueryAll.Where(p => !onlyPublished || p.IsPublished).OrderByDescending(p => p.Created);
		}

		public IQueryable<Post> GetForSearch(string search)
		{
            return QueryAll
                .Where(p => p.IsPublished)
                .Where(p => p.Title.ToLower().Contains(search.ToLower()))
                .OrderByDescending(p => p.Created);
        }

		public IQueryable<Post> GetForTag(string tagName)
		{
            return QueryAll
                .Where(p => p.IsPublished)
                .Where(p => p.Tags.Any(t => t.Name.ToLower().Equals(tagName.ToLower())))
                .OrderByDescending(p => p.Created);
        }
	}
}
