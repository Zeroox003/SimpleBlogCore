using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SimpleBlogCore.Domain.Entities
{
    /// <summary>
    /// Represents a blog entry - article, presentation or any thing
    /// </summary>
    public class Post : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string PreviewContent { get; set; }

        /// <summary>
        /// Flag to represent whether the article is visible or not
        /// </summary>
        public bool IsPublished { get; set; }

        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Collection of tags labelled over the post
        /// </summary>
        public virtual ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// List of comments left by users
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        public Post()
        { }
    }
}
