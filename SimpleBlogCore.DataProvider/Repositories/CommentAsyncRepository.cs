using Microsoft.EntityFrameworkCore;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogCore.DataProvider.Repositories
{
	public class CommentAsyncRepository : EfAsyncBaseRepository<Comment>, ICommentAsyncRepository
	{
		public CommentAsyncRepository(SimpleBlogDbContext context) : base(context)
		{
		}

		public async Task<IReadOnlyCollection<Comment>> GetAllForPost(Guid postId)
		{
			return await QueryAll.Where(c => c.PostId == postId).OrderBy(c => c.Created).ToListAsync();
		}
	}
}
