using System.Collections.Generic;
using Jahshaka.API.ViewModels.Application;
using Jahshaka.Core.Data;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static ApplicationVersionViewModel ToViewModel(this ApplicationVersion source)
        {
            if (source == null)
            {
                return null;
            }

            var destination = new ApplicationVersionViewModel();

            destination.Id = source.Id;
            destination.ApplicationId = source.ApplicationId;
            destination.Supported = source.Supported;
            destination.CreatedAt = source.CreatedAt;
            destination.UpdatedAt = source.UpdatedAt;
            destination.Notes = source.Notes;
            destination.DownloadUrl = source.DownloadUrl;

            return destination;
        }

        public static IList<ApplicationVersionViewModel> ToViewModel(this IList<ApplicationVersion> source)
        {
            var destination = new List<ApplicationVersionViewModel>();

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