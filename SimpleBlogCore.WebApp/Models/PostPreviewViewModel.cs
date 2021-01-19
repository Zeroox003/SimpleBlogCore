using SimpleBlogCore.Domain.Entities;
using System;

namespace SimpleBlogCore.WebApp.Models
{
    public class PostPreviewViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string PreviewContent { get; set; }

        public DateTime LastModified { get; set; }

        public bool IsEdited { get; set; }

        public PostPreviewViewModel()
        {

        }

        public PostPreviewViewModel(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            PreviewContent = post.PreviewContent;
            LastModified = post.LastModified ?? post.Created;
            IsEdited = post.LastModified != null;
        }
    }
}
