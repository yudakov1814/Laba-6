using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba_6.Data;
using Laba_6.Models;
using Microsoft.AspNetCore.Authorization;
using Laba_6.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Laba_6.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PostsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Posts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(x => x.Topic)
                .Include(x => x.Author)
                .Include(x => x.Replies)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var model = new PostViewModel(post, User);

            return View(model);
        }

        // GET: Posts/Create
        public IActionResult Create(int topicId)
        {
            var topic = _context.Topics.Find(topicId);

            if (topic == null)
            {
                return NotFound();
            }

            var post = new PostCreateModel
            {
                TopicId = topic.Id,
                TopicName = topic.Name
            };

            return View(post);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Text,TopicId,TopicName")] PostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var topic = _context.Topics.Find(model.TopicId);

                if (topic == null)
                {
                    return NotFound();
                }

                var DateTimeNow = DateTime.Now;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var post = new Post
                {
                    TopicId = topic.Id,
                    Title = model.Title,
                    Author = user,
                    CreatedTime = DateTimeNow,
                    Replies = new List<Reply>
                    {
                        new Reply
                        {
                            Text = model.Text,
                            Author = user,
                            CreatedTime = DateTimeNow,
                            EditTime = DateTimeNow,
                        }
                    }
                };

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TopicsController.Details), new { id = topic.Id, controller = "Topics" });
            }
            return View(model);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("admin") && User.Identity.Name != post.Author.UserName)
            {
                return Forbid();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (!User.IsInRole("admin") && User.Identity.Name != post.Author.UserName)
            {
                return Forbid();
            }

            var topicId = post.TopicId;
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TopicsController.Details), new { id = topicId, controller = "Topics" });
        }
    }
}
