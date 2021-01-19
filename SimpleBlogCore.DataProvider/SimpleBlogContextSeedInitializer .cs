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
                },
                new Tag {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Name = "C#",
                }
            };
        }

        private static List<Post> GetPosts(ICollection<Tag> tags)
        {
            return Enumerable.Range(1, 20).Select(GetPost).ToList();

            Post GetPost(int idx) {
                return new Post {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Title = "Article about programming " + idx,
                    Content = @"Lorem ipsum Nisi enim est proident est magna occaecat dolore proident eu ex sunt
                        consectetur consectetur dolore enim nisi exercitation adipisicing magna culpa commodo deserunt ut do Ut occaecat.
                        Lorem ipsum Veniam consequat quis aliquip dolore minim ex labore dolor Excepteur Duis velit in officia Excepteur
                        officia officia officia adipisicing magna eu ex sunt.

                        Duis ex ad cupidatat tempor Excepteur cillum cupidatat fugiat nostrud cupidatat dolor sunt sint sit nisi est
                        eu exercitation incididunt adipisicing veniam velit id fugiat enim mollit amet anim veniam dolor dolor irure 
                        velit commodo cillum sit nulla ullamco magna amet magna cupidatat qui labore cillum sit in tempor veniam consequat
                        non laborum adipisicing aliqua ea nisi sint ut quis proident ullamco ut dolore culpa occaecat ut laboris in sit
                        minim cupidatat ut dolor voluptate enim veniam consequat occaecat fugiat in adipisicing in amet Ut nulla nisi 
                        non ut enim aliqua laborum mollit quis nostrud sed sed.

                        Lorem ipsum Nisi enim est proident est magna occaecat dolore proident eu ex sunt consectetur consectetur dolore 
                        enim nisi exercitation adipisicing magna culpa commodo deserunt ut do Ut occaecat. Lorem ipsum Veniam consequat 
                        quis aliquip dolore minim ex labore dolor Excepteur Duis velit in officia Excepteur officia officia officia cillum
                        ut elit in fugiat incididunt ea ad Ut ut ea ea dolor ex dolor eu magna voluptate irure consectetur.

                        Placeat quam fugit qui quia.Non quasi tempore qui illo.Dolor magni ducimus doloribus rerum dolorem. Cum iste et 
                        commodi doloremque.At veniam aperiam eum voluptates maiores iure facere. Cupiditate vero similique ut sed aut.
                        Est sint laboriosam quia totam fugit. Necessitatibus sed ut autem eveniet mollitia. Temporibus ducimus officiis
                        aut est quaerat fuga est ut aut.",
                    PreviewContent = @"Lorem ipsum Nisi enim est proident est magna occaecat dolore proident eu ex sunt
                        consectetur consectetur dolore enim nisi exercitation adipisicing magna culpa commodo deserunt ut do Ut occaecat.
                        Lorem ipsum Veniam consequat quis aliquip dolore minim ex labore dolor Excepteur Duis velit in officia Excepteur
                        officia officia officia adipisicing magna eu ex sunt.",
                    IsPublished = true,
                    Tags = tags
                };
            } 
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
