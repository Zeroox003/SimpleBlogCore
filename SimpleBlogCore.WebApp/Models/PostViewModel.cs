using SimpleBlogCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBlogCore.WebApp.Models
{
    public class PostViewModel
    {
        public Guid? Id { get; set; }

        public DateTime? Created { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? LastModified { get; set; }

        public ICollection<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        public ICollection<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public PostViewModel()
        {

        }

        public PostViewModel(Post post)
        {
            Id = post.Id;
            Created = post.Created;
            Title = post.Title;
            Content = post.Content;
            IsPublished = post.IsPublished;
            LastModified = post.LastModified;
            
            foreach(var tag in post.Tags)
            {
                Tags.Add(new TagViewModel(tag));
            }

            foreach(var comment in post.Comments.OrderBy(c => c.Created))
            {
                Comments.Add(new CommentViewModel(comment));
            }
        }
    }
}
