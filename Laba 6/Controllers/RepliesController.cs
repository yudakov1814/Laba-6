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
using Microsoft.AspNetCore.Identity;

namespace Laba_6.Controllers
{
    [Authorize]
    public class RepliesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RepliesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction(nameof(TopicsController.Index), new { controller = "Topics" });
        }

        // POST: Replies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,PostId")] Reply reply)
        {
            if (ModelState.IsValid)
            {
                var DateTimeNow = DateTime.Now;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                reply.CreatedTime = DateTimeNow;
                reply.EditTime = DateTimeNow;
                reply.Author = user;

                _context.Add(reply);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PostsController.Details), new { id = reply.PostId, controller = "Posts" });
        }

        // GET: Replies/Edit/5
        public IActionResult Edit()
        {
            return RedirectToAction(nameof(TopicsController.Index), new { controller = "Topics" });
        }

        // POST: Replies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Reply reply)
        {
            var newText = reply.Text;
            reply = _context.Replies.Find(reply.Id);

            if (reply == null)
            {
                return NotFound();
            }

            var postId = reply.PostId;

            if (ModelState.IsValid && reply.Text != newText)
            {
                reply.Text = newText;
                reply.EditTime = DateTime.Now;
                _context.Update(reply);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PostsController.Details), new { id = postId, controller = "Posts" });
        }
    }
}
