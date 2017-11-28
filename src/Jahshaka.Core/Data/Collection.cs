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
        public Guid? UserId { get; set; }
        public string Name { get; set;  }
        public int? CollectionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual Collection CollectionParent { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}