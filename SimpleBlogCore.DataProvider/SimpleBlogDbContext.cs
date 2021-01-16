using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleBlogCore.Domain.Entities;
using System;

namespace SimpleBlogCore.DataProvider
{
    public class SimpleBlogDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public SimpleBlogDbContext(DbContextOptions<SimpleBlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(SimpleBlogDbContext).Assembly);
        }
    }
}
