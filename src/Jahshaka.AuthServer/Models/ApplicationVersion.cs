using System;

namespace Jahshaka.AuthServer.Models
{
    public class ApplicationVersion
    {
        public string Id { get; set; }

        public Guid ApplicationId { get; set; }

        public bool Supported { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Application Application { get; set; }
    }
}