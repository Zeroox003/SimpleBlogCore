using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using SimpleBlogCore.DataProvider.Extensions;
using System;
using System.Threading.Tasks;
using System.Linq;
using SimpleBlogCore.WebApp.Models;

namespace SimpleBlogCore.WebApp.Controllers
{
	[Authorize]
	public class CommentController : Controller
	{
		private readonly ICommentAsyncRepository _commentRepository;

		public CommentController(ICommentAsyncRepository commentRepository)
		{
			_commentRepository = commentRepository;
		}

		[HttpPost]
		public async Task<IActionResult> Add(string commentText, Guid? repliedToCommentId, Guid postId)
		{
			var newComment = new Comment {
				Id = Guid.NewGuid(),
				Created = DateTime.UtcNow,
				Content = commentText,
				IsDeleted = false,
				PostId = postId,
				RepliedToCommentId = repliedToCommentId,
				UserId = User.Identity.GetUserId()
			};

			await _commentRepository.Add(newComment);

			var comments = (await _commentRepository.GetAllForPost(postId))
				.Select(c => new CommentViewModel(c));
			return PartialView("_CommentsPartial", comments);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Guid commentId, string commentText)
		{
			var comment = await _commentRepository.GetById(commentId);
			comment.Content = commentText;
			await _commentRepository.Update(comment);

			var comments = (await _commentRepository.GetAllForPost(comment.PostId))
				.Select(c => new CommentViewModel(c));
			return PartialView("_CommentsPartial", comments);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid commentId)
		{
			var currentComment = await _commentRepository.GetById(commentId);

			var comments = (await _commentRepository.GetAllForPost(currentComment.PostId)).ToList();
			if (comments.Any(c => c.RepliedToCommentId == currentComment.Id))
			{
				currentComment.IsDeleted = true;
				await _commentRepository.Update(currentComment);
			}
			else
			{
				// TODO: If currentComment has a RepliedToComment with IsDeleted set to true, then remove that too
				await _commentRepository.Remove(currentComment);
				comments.Remove(currentComment);
			}

			return PartialView("_CommentsPartial", comments.Select(c => new CommentViewModel(c)));
		}
	}
}
