using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBlogCore.Domain.Entities;
using System.Collections.Generic;

namespace SimpleBlogCore.DataProvider.EntityConfigurations
{
    internal class PostConfiguration : BaseEntityConfiguration<Post>
    {
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Content).IsRequired();
            builder.Property(p => p.PreviewContent).IsRequired().HasMaxLength(600);

            builder.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    e => e.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade),
                    e => e.HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.ClientCascade));

            builder.Property(x => x.IsPublished)
                .HasDefaultValue(false);
        }
    }
}
