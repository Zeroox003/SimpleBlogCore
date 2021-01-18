using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlogCore.Domain.Entities
{
    /// <summary>
    /// Represents a tag that is labelled on a post. Helps to find the wanted posts
    /// </summary>
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// Additional information or tag transcript
        /// </summary>
        public string Description { get; set; }

        public ICollection<Post> Posts { get; set; }

        public Tag()
        { }
    }
}
