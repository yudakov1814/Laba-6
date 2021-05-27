using Laba_6.Data;
using Laba_6.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laba_6
{
    public class DBInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            if (await roleManager.FindByNameAsync("admin") == null &&
                await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                await roleManager.CreateAsync(new IdentityRole("user"));

                var admin = new IdentityUser { Email = "admin@mail.com", UserName = "admin@mail.com" };
                var result = await userManager.CreateAsync(admin, "admin");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }

                var simpleUser = new IdentityUser { Email = "user@mail.com", UserName = "user@mail.com" };
                result = await userManager.CreateAsync(simpleUser, "user");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(simpleUser, "user");
                }

                var DateTimeNow = DateTime.Now;

                var topic1 = new Topic
                {
                    Name = "Cars",
                    CreatedTime = DateTimeNow.AddDays(-0.5),
                    Author = admin,
                    Posts = new List<Post>
                    {
                        new Post
                        {
                            Title = "Your favorite cars",
                            CreatedTime = DateTimeNow.AddDays(-0.4),
                            Author = admin,
                            Replies = new List<Reply>
                            {
                                new Reply
                                {
                                    Text = "Tell me what kind of cars you like",
                                    CreatedTime = DateTimeNow.AddDays(-0.4),
                                    EditTime = DateTimeNow.AddDays(-0.4),
                                    Author = admin
                                },
                                new Reply
                                {
                                    Text = "I love fast cars",
                                    CreatedTime = DateTimeNow.AddDays(-0.3),
                                    EditTime = DateTimeNow.AddDays(-0.3),
                                    Author = simpleUser
                                },
                                new Reply
                                {
                                    Text = "Yes, I also like fast cars",
                                    CreatedTime = DateTimeNow.AddDays(-0.2),
                                    EditTime = DateTimeNow.AddDays(-0.2),
                                    Author = admin
                                }
                            }
                        }
                    }
                };

                var topic2 = new Topic
                {
                    Name = "Programming",
                    CreatedTime = DateTimeNow.AddDays(-1),
                    Author = admin,
                    Posts = new List<Post>
                    {
                        new Post
                        {
                            Title = "Help me with the task",
                            CreatedTime = DateTimeNow.AddDays(-0.8),
                            Author = admin,
                            Replies = new List<Reply>
                            {
                                new Reply
                                {
                                    Text = "How to add two numbers in c++?",
                                    CreatedTime = DateTimeNow.AddDays(-0.8),
                                    EditTime = DateTimeNow.AddDays(-0.8),
                                    Author = admin
                                },
                                new Reply
                                {
                                    Text = "mb 'a + b'?",
                                    CreatedTime = DateTimeNow.AddDays(-0.7),
                                    EditTime = DateTimeNow.AddDays(-0.7),
                                    Author = simpleUser
                                },
                                new Reply
                                {
                                    Text = "Can you write the whole program?",
                                    CreatedTime = DateTimeNow.AddDays(-0.65),
                                    EditTime = DateTimeNow.AddDays(-0.64),
                                    Author = admin
                                },
                                new Reply
                                {
                                    Text = "ok",
                                    CreatedTime = DateTimeNow.AddDays(-0.6),
                                    EditTime = DateTimeNow.AddDays(-0.6),
                                    Author = simpleUser
                                },
                                new Reply
                                {
                                    Text = "int a; int b; cin >> a >> b; cout << a + b;",
                                    CreatedTime = DateTimeNow.AddDays(-0.59),
                                    EditTime = DateTimeNow.AddDays(-0.58),
                                    Author = simpleUser
                                },
                                new Reply
                                {
                                    Text = "Thanks a lot",
                                    CreatedTime = DateTimeNow.AddDays(-0.5),
                                    EditTime = DateTimeNow.AddDays(-0.5),
                                    Author = admin
                                },
                                new Reply
                                {
                                    Text = "You are welcome :)",
                                    CreatedTime = DateTimeNow.AddDays(-0.45),
                                    EditTime = DateTimeNow.AddDays(-0.45),
                                    Author = simpleUser
                                }
                            }
                        },
                        new Post
                        {
                            Title = "Best programming language",
                            CreatedTime = DateTimeNow.AddMonths(-10),
                            Author = simpleUser,
                            Replies = new List<Reply>
                            {
                                new Reply
                                {
                                    Text = "What is the best programming language?",
                                    CreatedTime = DateTimeNow.AddMonths(-10),
                                    EditTime = DateTimeNow.AddMonths(-10),
                                    Author = simpleUser
                                }
                            }
                        }
                    }
                };

                context.Add(topic1);
                context.Add(topic2);
                await context.SaveChangesAsync();
            }
        }
    }
}
