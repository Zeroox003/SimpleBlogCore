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

        public async Task<IReadOnlyList<Post>> GetAll(bool onlyPublished = true)
        {
            return await QueryAll.Where(p => !onlyPublished || p.IsPublished).OrderByDescending(p => p.Created).ToListAsync();
        }
    }
}
