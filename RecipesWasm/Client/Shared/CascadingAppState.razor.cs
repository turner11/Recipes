using Components.Recipes;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using RecipesWasm.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWasm.Client.Shared
{
    public partial class CascadingAppState
    {
        [Inject]
        Services.IRecipesProvider RecipesProvider { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        IMatToaster Toaster { get; set; }
        public ReadOnlyCollection<RecipeViewModel> AllRecipes { get; private set; }
        public ReadOnlyCollection<RecipeViewModel> Recipes => GetFilteredRecipes(this.Filters);

      

        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> LabelCategories { get; private set; }
        Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var toast = this.Toaster.Add("Getting recipes", MatToastType.Info); ;
                try
                {
                    IReadOnlyList<IRecipe> recipes = await this.RecipesProvider.GetRecipes();
                    this.AllRecipes = recipes.Select(r => new RecipeViewModel(r)).ToList().AsReadOnly();                    
                    toast.InvokeOnClose();



                    var filters = this.Recipes.SelectMany(r => r.Labels)
                                                .GroupBy(lbl => lbl.Category)
                                                .ToDictionary(group => group.Key, 
                                                                group => group.Select(v => v.Title).Distinct().ToList().AsReadOnly());

                    this.LabelCategories = new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(filters);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    this.Toaster.Add(ex.Message, MatToastType.Danger, title: "Error getting Recipes"); ;
                }

                await this.InvokeAsync(this.StateHasChanged);
            }
        }

        internal void SetFilters(IDictionary<string, string> filters)
        {
            this.Filters = new Dictionary<string, string>(filters);
            this.StateHasChanged();
        }

        private ReadOnlyCollection<RecipeViewModel> GetFilteredRecipes(Dictionary<string, string> filters)
        {
            ReadOnlyCollection<RecipeViewModel> recipes = this.AllRecipes;
            var _filters = filters?.Where(p => !String.IsNullOrWhiteSpace(p.Value) && !String.IsNullOrWhiteSpace(p.Key))
                                  ?.ToDictionary(p=> p.Key, p=>p.Value);
            if (_filters is null || recipes is null || _filters.Count == 0)
                return recipes ?? new List<RecipeViewModel>().AsReadOnly();

            bool hasMatch(RecipeViewModel r)
            {
                
                bool isMatch = true;
                foreach ((var category, var value) in _filters)
                {
                    IEnumerable<Label> relevantLabels = r.Labels.Where(lbl=> lbl.Category.Equals(category, StringComparison.InvariantCultureIgnoreCase));
                    isMatch &= relevantLabels.Any(lbl => lbl.Title.Equals(value, StringComparison.InvariantCultureIgnoreCase));
                }

                
                return isMatch;

            }

            var filtered = recipes.Where(hasMatch).ToList();
            return filtered.AsReadOnly();
        }

    }
}
