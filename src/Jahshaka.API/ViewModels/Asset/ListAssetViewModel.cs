using System.Collections.Generic;

namespace Jahshaka.API.ViewModels.Asset
{
    public class ListAssetViewModel
    {
        public List<AssetViewModel> Assets { get; set; }

        public string query { get; set; }
    }
}