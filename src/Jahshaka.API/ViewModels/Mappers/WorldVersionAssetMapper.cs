using System.Collections.Generic;
using Jahshaka.API.ViewModels.Asset;
using Jahshaka.API.ViewModels.World;
using Jahshaka.Core.Data;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static AssetViewModel ToViewModel(this WorldVersionAsset source)
        {
            if(source == null)
            {
                return null;
            }

            var destination = new AssetViewModel
            {
                Id = source.Asset.Id,
                Name = source.Asset.Name,
                Type = source.Asset.Type,
                Url = source.Asset.Url,
                IconUrl = source.Asset.IconUrl,
                IsPublic = source.Asset.IsPublic,
                MetaData = source.Asset.MetaData,
                Tags = source.Asset.Tags,
                UserId = source.Asset.UserId,
                CreatedAt = source.Asset.CreatedAt
            };


            return destination;
        }

        public static List<AssetViewModel> ToViewModel(this List<WorldVersionAsset> source)
        {
            var destination = new List<AssetViewModel>();

            if(source != null)
            {
                foreach(var item in source)
                {
                    destination.Add(item.ToViewModel());
                }
            }

            return destination;
        }
    }
}