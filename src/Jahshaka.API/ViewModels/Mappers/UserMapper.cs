using System.Collections.Generic;
using Jahshaka.API.ViewModels.User;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static UserViewModel ToViewModel(this Core.Data.ApplicationUser source)
        {
            if(source == null)
            {
                return null;
            }

            var destination = new UserViewModel();

            destination.Id = source.Id;
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.UserName = source.UserName;
            destination.Email = source.Email;
            destination.CreatedAt = source.CreatedAt;

            return destination;
        }

        public static List<UserViewModel> ToViewModel(this List<Core.Data.ApplicationUser> source)
        {
            var destination = new List<UserViewModel>();

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