using SimpleBlogCore.Domain.Entities;
using System;

namespace SimpleBlogCore.WebApp.Models
{
    public class TagViewModel
    {
        public Guid? Id { get; set; }

        public DateTime? Created { get; set; }

        public string Name { get; set; }

        public string UrlSlug { get; set; }

        public string Description { get; set; }

        public TagViewModel()
        {

        }

        public TagViewModel(Tag tag)
        {
            Id = tag.Id;
            Created = tag.Created;
            Name = tag.Name;
            Description = tag.Description;
        }

        public Tag GetEntity()
        {
            return new Tag {
                Id = Id ?? Guid.NewGuid(),
                Created = Created ?? DateTime.UtcNow,
                Name = Name,
                Description = Description
            };
        }
    }
}
