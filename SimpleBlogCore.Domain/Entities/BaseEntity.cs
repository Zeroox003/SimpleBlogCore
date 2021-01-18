using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlogCore.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Created { get; set; }
    }
}
