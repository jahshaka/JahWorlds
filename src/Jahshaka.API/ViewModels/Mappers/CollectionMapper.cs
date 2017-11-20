using System.Collections.Generic;
using System.Linq;
using Jahshaka.API.ViewModels.Collection;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static CollectionViewModel ToViewModel(this Core.Data.Collection source)
        {
            if(source == null)
            {
                return null;
            }

            var destination = new CollectionViewModel();

            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.CreatedAt = source.CreatedAt;

            if(source.Collections != null){
                destination.Collections = source.Collections.ToList().ToViewModel();
            }

            if(source.Assets != null){
                destination.Assets = source.Assets.ToList().ToViewModel();
            }

            return destination;
        }

        public static List<CollectionViewModel> ToViewModel(this List<Core.Data.Collection> source)
        {
            var destination = new List<CollectionViewModel>();

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