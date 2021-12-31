using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RecipesWasm.Client.Shared
{
    public partial class ToolBar
    {
        [CascadingParameter] public CascadingAppState AppState { get; set; }

        ReadOnlyDictionary<string, ReadOnlyCollection<string>> AllFilters => AppState.LabelCategories;

        private ConcurrentDictionary<string, string> _filters;

        public ConcurrentDictionary<string, string> filters
        {
            get
            {
                _filters ??= new ConcurrentDictionary<string, string>(AllFilters?.Keys.ToDictionary(k => k, k => ""));
                return _filters;
            }
            set { _filters = value; }
        }

        

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);            
        }


        void SetFiltered(string category, string title)
        {
            filters[category] = title;
            this.AppState.SetFilters(filters);
        }



    }
}
