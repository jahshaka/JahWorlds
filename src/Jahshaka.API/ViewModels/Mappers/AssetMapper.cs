using System.Collections.Generic;
using Jahshaka.API.ViewModels.Asset;
using System.Reflection;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static AssetViewModel ToViewModel(this Core.Data.Asset source)
        {
            if(source == null)
            {
                return null;
            }

            var destination = new AssetViewModel();

            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Type = source.Type.ToString();
            destination.Url = source.Url;
            destination.IconUrl = source.IconUrl;
            destination.IsPublic = source.IsPublic;
            destination.MetaData = source.MetaData;
            destination.Tags = source.Tags;
            destination.UserId = source.UserId;
            destination.CollectionId = source.CollectionId;
            destination.CreatedAt = source.CreatedAt;

            return destination;
        }

        public static List<AssetViewModel> ToViewModel(this List<Core.Data.Asset> source)
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