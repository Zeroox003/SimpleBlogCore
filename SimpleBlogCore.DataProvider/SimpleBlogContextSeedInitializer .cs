using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlogCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBlogCore.DataProvider
{
    public static class SimpleBlogContextSeedInitializer
    {
        public static void ApplySeed(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceProvider.GetService<SimpleBlogDbContext>())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    var (admin, user) = GetUsers();
                    context.Users.AddRange(admin, user);

                    var (adminRole, userRole) = GetRoles();
                    context.Roles.AddRange(adminRole, userRole);

                    var userRoleMap = new List<(User, IdentityRole<Guid>)> {
                        (admin, adminRole),
                        (user, userRole),
                    };
                    context.UserRoles.AddRange(GetUserRolesMap(userRoleMap));

                    var tags = GetTags();
                    context.Tags.AddRange(tags);

                    var posts = GetPosts(tags);
                    context.Posts.AddRange(posts);

                    var comments = GetComments(posts.First(), (admin, user));
                    context.Comments.AddRange(comments);

                    context.SaveChanges();
                }
            }
        }

        private static (User Admin, User User) GetUsers()
        {
            var hasher = new PasswordHasher<User>();
            return (
                new User {
                    Id = Guid.NewGuid(),
                    RegistrationDate = DateTime.UtcNow,
                    UserName = "Admin",
                    NormalizedUserName = "admin",
                    Email = "admin@mail.ru",
                    NormalizedEmail = "admin@mail.ru",
                    PasswordHash = hasher.HashPassword(null, "admin123"),
                    SecurityStamp = string.Empty,
                    EmailConfirmed = true
                },
                new User {
                    Id = Guid.NewGuid(),
                    RegistrationDate = DateTime.UtcNow,
                    UserName = "User",
                    NormalizedUserName = "user",
                    Email = "user@mail.ru",
                    NormalizedEmail = "user@mail.ru",
                    PasswordHash = hasher.HashPassword(null, "user123"),
                    SecurityStamp = string.Empty,
                    EmailConfirmed = true
                });
        }

        private static (IdentityRole<Guid> AdminRole, IdentityRole<Guid> UserRole) GetRoles()
        {
            return (
                new IdentityRole<Guid> {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = "admin"
                },
                new IdentityRole<Guid> {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    NormalizedName = "user"
                });
        }

        private static List<IdentityUserRole<Guid>> GetUserRolesMap(IEnumerable<(User User, IdentityRole<Guid> Role)> userRoleMap)
        {
            return userRoleMap.Select(map =>
                new IdentityUserRole<Guid> { 
                    UserId = map.User.Id,
                    RoleId = map.Role.Id 
                }).ToList();
        }

        private static List<Tag> GetTags()
        {
            return new List<Tag> {
                new Tag {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Name = "Programming",
                }
            };
        }

        private static List<Post> GetPosts(ICollection<Tag> tags)
        {
            return new List<Post> {
                new Post {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Title = "Article about programming",
                    Body = "Some text...",
                    IsPublished = true,
                    Tags = tags
                }
            };
        }

        private static List<Comment> GetComments(Post post, (User Admin, User User) users)
        {
            var firstComment = new Comment {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Content = "It's great article!",
                PostId = post.Id,
                UserId = users.User.Id
            };

            return new List<Comment> {
                firstComment,
                new Comment {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Content = "Yeah, I made an effort...",
                    PostId = post.Id,
                    RepliedToCommentId = firstComment.Id,
                    UserId = users.Admin.Id
                }
            };
        }
    }
}
