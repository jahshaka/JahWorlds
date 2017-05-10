using System.Collections.Generic;
using Jahshaka.Web.ViewModels.Asset;

namespace Jahshaka.Web.ViewModels.Mappers
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
            destination.Type = source.Type;
            destination.Url = source.Url;
            destination.IconUrl = source.IconUrl;
            destination.IsPublic = source.IsPublic;
            destination.MetaData = source.MetaData;
            destination.Tags = source.Tags;
            destination.UserId = source.UserId;
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