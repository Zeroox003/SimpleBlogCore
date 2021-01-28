using SimpleBlogCore.Domain.Entities;
using System;

namespace SimpleBlogCore.WebApp.Models
{
    public class CommentViewModel
    {
        public Guid? Id { get; set; }

        public DateTime? Created { get; set; }

        public string Content { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? UserId { get; set; }

        public User User { get; set; }

        public Guid PostId { get; set; }

        public Guid? RepliedToCommentId { get; set; }

        public CommentViewModel RepliedToComment { get; set; }

        public CommentViewModel()
        {

        }

        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            Created = comment.Created;
            Content = comment.Content;
            IsDeleted = comment.IsDeleted;
            UserId = comment.UserId;
            User = comment.User;
            PostId = comment.PostId;
            RepliedToCommentId = comment.RepliedToCommentId;
            if (comment.RepliedToComment != null)
            {
                RepliedToComment = new CommentViewModel(comment.RepliedToComment);
            }
        }

        public Comment GetEntity()
        {
            return new Comment {
                Id = Id ?? Guid.NewGuid(),
                Created = Created ?? DateTime.UtcNow,
                Content = Content,
                IsDeleted = IsDeleted,
                UserId = UserId,
                PostId = PostId,
                RepliedToCommentId = RepliedToCommentId
            };
        }
    }
}
