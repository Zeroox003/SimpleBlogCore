using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlogCore.Domain.Entities
{
    /// <summary>
    /// The comment left by the user under the post
    /// </summary>
    public class Comment : BaseEntity
    {
        public string Content { get; set; }

        /// <summary>
        /// Flag to represent whether the comment content is visible or not
        /// </summary>
        public bool IsDeleted { get; set; }

        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        /// <summary>
        /// Refence to reply to parent comment
        /// </summary>
        public Guid? RepliedToCommentId { get; set; }
        public virtual Comment RepliedToComment { get; set; }

        public Comment()
        { }
    }
}
