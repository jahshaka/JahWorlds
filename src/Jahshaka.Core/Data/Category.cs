using System;
using System.Collections.Generic;

namespace Jahshaka.Core.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set;  }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
}