using Laba_6.Data;
using Laba_6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laba_6.ViewModels
{
    public class ImageFileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<Folder> Parents { get; set; }

        public ImageFileViewModel(ImageFile image, ApplicationDbContext context)
        {
            Id = image.Id;
            Name = image.Name;
            Path = image.Path;
            Parents = new List<Folder>();

            GetParents(image.Folder, context);
        }

        private void GetParents(Folder folder, ApplicationDbContext context)
        {
            var parent = context.Folders.Find(folder.ParentId);

            if (parent != null)
            {
                GetParents(parent, context); 
            }

            Parents.Add(folder);
        }
    }
}
