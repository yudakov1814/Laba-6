using Laba_6.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Laba_6.ViewModels
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string Title { get; set; }
        public ICollection<ReplyDto> Replies { get; set; }
        public bool CanEdit { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Text { get; set; }

        public PostViewModel(Post post, ClaimsPrincipal user)
        {
            PostId = post.Id;
            TopicId = post.TopicId;
            TopicName = post.Topic.Name;
            Title = post.Title;
            Replies = post.Replies.Select(x => new ReplyDto {
                Id = x.Id,
                Text = x.Text,
                CreatedTime = x.CreatedTime,
                EditTime = x.EditTime,
                Author = x.Author,
                CanEdit = user.IsInRole("admin") || user.Identity.Name == x.Author.UserName
            }).ToList();
            CanEdit = user.IsInRole("admin") || user.Identity.Name == post.Author.UserName;
        }

        public class ReplyDto
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public DateTime CreatedTime { get; set; }
            public DateTime EditTime { get; set; }
            public IdentityUser Author { get; set; }
            public bool CanEdit { get; set; }
        }
    }
}
