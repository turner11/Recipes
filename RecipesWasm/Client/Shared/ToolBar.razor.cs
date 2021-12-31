using Microsoft.AspNetCore.Components;
using RecipesWasm.Shared;
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
        public List<Label> filters { get; private set; } = new List<Label>();
        

        

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);            
        }


        void SetFiltered(string category, string title)
        {
            filters = filters.Where(lbl => !lbl.Category.Equals(category, System.StringComparison.InvariantCultureIgnoreCase)).ToList();
            filters.Add(new Label(category, title));
            this.AppState.SetFilters(filters);
        }



    }
}
