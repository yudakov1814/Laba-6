using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laba_6.Models
{
    public class ImageFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Folder Folder { get; set; }
        public int FolderId { get; set; }
    }
}
