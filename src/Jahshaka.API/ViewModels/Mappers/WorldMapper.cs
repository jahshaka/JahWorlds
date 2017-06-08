using System.Collections.Generic;
using Jahshaka.API.ViewModels.World;
using Jahshaka.Core.Data;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static WorldViewModel ToViewModel(this Core.Data.World source)
        {
            if(source == null)
            {
                return null;
            }

            var destination = new WorldViewModel
            {
                Id = source.Id,
                Name = source.Name,
                ThumbnailUrl = source.ThumbnailUrl,
                UserId = source.UserId,
                CreatedAt = source.CreatedAt,
                WorldVersions = (IList<WorldVersion>) source.WorldVersions
            };

            return destination;
        }

        public static List<WorldViewModel> ToViewModel(this List<Core.Data.World> source)
        {
            var destination = new List<WorldViewModel>();

            if(source != null)
            {
                foreach(var item in source)
                {
                    destination.Add(item.ToViewModel());
                }
            }

            return destination;
        }
        
        public static WorldVersionViewModel ToVersionViewModel(this WorldVersion source)
        {
            if(source == null)
            {
                return null;
            }

            var destination = new WorldVersionViewModel
            {
                VersionNumber = source.VersionNumber,
                World = source.World.ToViewModel()
            };

            return destination;
        }
    }
}