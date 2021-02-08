using SimpleBlogCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogCore.Domain.Interfaces
{
	public interface ICommentAsyncRepository : IAsyncRepository<Comment>
	{
		Task<IReadOnlyCollection<Comment>> GetAllForPost(Guid postId);
	}
}
