using SimpleBlogCore.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlogCore.WebApp.Models
{
    public class TagViewModel
    {
        public Guid? Id { get; set; }

        public DateTime? Created { get; set; }

        [Required]
        public string Name { get; set; }

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

        public void Update(Tag tag)
		{
            tag.Name = Name;
            tag.Description = Description;
		}
    }
}
