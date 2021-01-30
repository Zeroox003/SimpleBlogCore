using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBlogCore.Domain.Entities;

namespace SimpleBlogCore.DataProvider.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
            builder.Property(u => u.FullName).HasMaxLength(100);
            builder.Property(u => u.Address).HasMaxLength(100);
            builder.Property(u => u.Gender).HasMaxLength(50);
            builder.Property(u => u.ProfilePicturePath).IsRequired();

            builder.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
