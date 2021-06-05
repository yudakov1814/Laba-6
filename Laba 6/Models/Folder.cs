using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laba_6.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Folder Parent { get; set; }
        public int? ParentId { get; set; }
    }
}
