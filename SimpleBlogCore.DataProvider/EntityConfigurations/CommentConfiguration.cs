using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBlogCore.Domain.Entities;

namespace SimpleBlogCore.DataProvider.EntityConfigurations
{
    internal class CommentConfiguration : BaseEntityConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Content).IsRequired().HasMaxLength(500);
        }
    }
}
