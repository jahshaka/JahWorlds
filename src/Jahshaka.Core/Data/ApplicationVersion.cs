using System;

namespace Jahshaka.Core.Data
{
    public class ApplicationVersion
    {
        public string Id { get; set; }

        public Guid ApplicationId { get; set; }

        public bool Supported { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Notes { get; set; }

        public string DownloadUrl {get; set;}

        public string WindowsUrl {get; set;}

        public string MacUrl {get; set;}

        public string LinuxUrl {get; set;}

        public DateTime? ReleaseDate { get; set; }

        public Application Application { get; set; }
    }
}