using System.Collections.Generic;
using System.Linq;
using Jahshaka.API.ViewModels.Application;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static ApplicationViewModel ToViewModel(this Jahshaka.Core.Data.Application source)
        {
            if (source == null)
            {
                return null;
            }

            var destination = new ApplicationViewModel();

            destination.Id = source.Id;
            destination.ClientId = source.ClientId;
            destination.ClientSecret = source.ClientSecret;
            destination.DisplayName = source.DisplayName;
            destination.RedirectUris = source.RedirectUris;
            destination.PostLogoutRedirectUris = source.PostLogoutRedirectUris;
            destination.Type = source.Type;

            if (source.Versions != null)
            {
                destination.Versions = source.Versions.ToList().ToViewModel();
            }

            return destination;
        }

        public static List<ApplicationViewModel> ToViewModel(this List<Jahshaka.Core.Data.Application> source)
        {
            var destination = new List<ApplicationViewModel>();

            if (source != null)
            {
                foreach (var item in source)
                {
                    destination.Add(item.ToViewModel());
                }
            }

            return destination;
        }
    }
}