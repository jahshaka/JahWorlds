using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Jahshaka.Core.Data
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set;  }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }

    }
}