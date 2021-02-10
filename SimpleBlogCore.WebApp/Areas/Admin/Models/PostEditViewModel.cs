using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogCore.WebApp.Models
{
    public class PostEditViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Preview content")]
        public string PreviewContent { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsPublished { get; set; }

        [Display(Name = "Tags")]
        public ICollection<Guid> TagIds { get; set; } = new List<Guid>();

        public ICollection<Tag> AvailableTags { get; set; } = new List<Tag>();

		public PostEditViewModel()
		{

		}

        public PostEditViewModel(IEnumerable<Tag> tags)
        {
            AvailableTags = tags.ToList();
        }

        public PostEditViewModel(Post post, IEnumerable<Tag> tags) : this(tags)
        {
            Id = post.Id;
            Title = post.Title;
            PreviewContent = post.PreviewContent;
            Content = post.Content;
            IsPublished = post.IsPublished;
            TagIds = post.Tags.Select(t => t.Id).ToList();
        }

        public async Task<Post> GetEntity(IAsyncRepository<Tag> tagRepository)
        {
            var post = new Post {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Title = Title,
                PreviewContent = PreviewContent,
                Content = Content,
                IsPublished = IsPublished,
                LastModified = null,
                Tags = new List<Tag>()
            };

            foreach(var tagId in TagIds)
			{
                post.Tags.Add(await tagRepository.GetById(tagId));
			}

            return post;
        }

		public async Task Update(Post post, IAsyncRepository<Tag> tagRepository)
		{
            post.Title = Title;
            post.PreviewContent = PreviewContent;
            post.LastModified = DateTime.UtcNow;
            post.Content = Content;
            post.IsPublished = IsPublished;
            post.Tags.Clear();
            foreach (var tagId in TagIds)
            {
                post.Tags.Add(await tagRepository.GetById(tagId));
            }
        }
    }
}
