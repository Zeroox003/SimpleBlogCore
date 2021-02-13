using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlogCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
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
                    if (context.Database.EnsureCreated())
                    {

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
        }

        private static (User Admin, User User) GetUsers()
        {
            var hasher = new PasswordHasher<User>();
            var imagePlaceholder = "~/images/placeholder.png";
            return (
                new User {
                    Id = Guid.NewGuid(),
                    RegistrationDate = DateTime.UtcNow,
                    UserName = "Admin",
                    NormalizedUserName = "admin",
                    Email = "admin@mail.ru",
                    NormalizedEmail = "admin@mail.ru",
                    PasswordHash = hasher.HashPassword(null, "admin123"),
                    ProfilePicturePath = imagePlaceholder,
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
                    ProfilePicturePath = imagePlaceholder,
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
            return Enumerable.Range(1, 30).Select(GetPost).ToList();

            Post GetPost(int idx) {
                return new Post {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Title = "Article about programming " + idx,
                    Content = @"<p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut 
                        laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl 
                        ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum 
                        dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue
                        duis dolore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim
                        placerat facer possim assum. Typi non habent claritatem insitam; est usus legentis in iis qui facit eorum claritatem. Investigationes
                        demonstraverunt lectores legere me lius quod ii legunt saepius. Claritas est etiam processus dynamicus, qui sequitur mutationem 
                        consuetudium lectorum. Mirum est notare quam littera gothica, quam nunc putamus parum claram, anteposuerit litterarum formas 
                        humanitatis per seacula quarta decima et quinta decima. Eodem modo typi, qui nunc nobis videntur parum clari, fiant sollemnes in
                        futurum.</p><p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore
                        magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex 
                        ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu 
                        feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore
                        te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat 
                        facer possim assum. Typi non habent claritatem insitam; est usus legentis in iis qui facit eorum claritatem. Investigationes 
                        demonstraverunt lectores legere me lius quod ii legunt saepius. Claritas est etiam processus dynamicus, qui sequitur mutationem 
                        consuetudium lectorum. Mirum est notare quam littera gothica, quam nunc putamus parum claram, anteposuerit litterarum formas humanitatis 
                        per seacula quarta decima et quinta decima. Eodem modo typi, qui nunc nobis videntur parum clari, fiant sollemnes in futurum.</p><p>Lorem
                        ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. 
                        Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem 
                        vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et 
                        accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Nam liber tempor
                        cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum. Typi non habent claritatem 
                        insitam; est usus legentis in iis qui facit eorum claritatem. Investigationes demonstraverunt lectores legere me lius quod ii legunt 
                        saepius. Claritas est etiam processus dynamicus, qui sequitur mutationem consuetudium lectorum. Mirum est notare quam littera gothica,
                        quam nunc putamus parum claram, anteposuerit litterarum formas humanitatis per seacula quarta decima et quinta decima. Eodem modo typi,
                        qui nunc nobis videntur parum clari, fiant sollemnes in futurum.</p>",
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
