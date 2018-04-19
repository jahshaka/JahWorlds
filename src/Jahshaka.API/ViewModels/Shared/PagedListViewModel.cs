using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Jahshaka.API.ViewModels.Shared
{
    public class PagedListViewModel<T>
    {
        public PagedListViewModel()
        {
            Items = new Collection<T>();

            Paging = new PagingOptionsViewModel()
            {
                CurrentPage = 0,
                TotalItems = 0,
                TotalPages = 0,
                NextUrl = null,
                PrevUrl = null
            };
        }

        [JsonProperty("items")]
        public ICollection<T> Items { get; set; }

        [JsonProperty("paging")]
        public PagingOptionsViewModel Paging { get; set; }
    }
}
