using Laba_6.Data;
using Laba_6.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Laba_6.ViewModels
{
    public class FolderViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public List<Folder> Parents { get; set; }
        public List<Folder> SubFolders { get; set; }
        public List<ImageFile> Images { get; set; }

        [Required(ErrorMessage = "Please select a file")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        public FolderViewModel(Folder folder, ApplicationDbContext context)
        {
            Id = folder.Id;
            ParentId = folder.ParentId;
            Name = folder.Name;
            Parents = new List<Folder>();

            GetParents(folder, context);
            GetImages(folder, context);
            GetSubFolders(folder, context);
        }

        private void GetParents(Folder folder, ApplicationDbContext context)
        {
            var parent = context.Folders.Find(folder.ParentId);

            if (parent != null)
            {
                GetParents(parent, context);
                Parents.Add(parent);
            }
        }

        private void GetImages(Folder folder, ApplicationDbContext context)
        {
            Images = context.ImageFiles.Where(x => x.FolderId == folder.Id).ToList();
        }

        private void GetSubFolders(Folder folder, ApplicationDbContext context)
        {
            SubFolders = context.Folders.Where(x => x.ParentId == folder.Id).ToList();
        }
    }
}
