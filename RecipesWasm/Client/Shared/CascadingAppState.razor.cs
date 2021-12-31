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
        List<Label> Filters { get;} = new List<Label>();

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
                                                                group => group.Select(v => v.Value).Distinct().ToList().AsReadOnly());

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

        internal void SetFilters(IList<Label> filters)
        {
            this.Filters.Clear();
            this.Filters.AddRange(filters);

            this.StateHasChanged();
        }

        private ReadOnlyCollection<RecipeViewModel> GetFilteredRecipes(IList<Label> filters)
        {
            ReadOnlyCollection<RecipeViewModel> recipes = this.AllRecipes;
            var _filters = filters?.Where(lbl => !String.IsNullOrWhiteSpace(lbl.Value) && !String.IsNullOrWhiteSpace(lbl.Category))?.ToList();
                                  
            if (_filters is null || recipes is null || _filters.Count == 0)
                return recipes ?? new List<RecipeViewModel>().AsReadOnly();

            bool hasMatch(RecipeViewModel r)
            {
                
                bool isMatch = true;
                foreach (Label filterLbl in _filters)
                {
                    var relevantLabels = r.Labels.Where(lbl=> lbl.Category.Equals(filterLbl.Category, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    isMatch &= relevantLabels.Any(lbl => lbl.Value.Equals(filterLbl.Value, StringComparison.InvariantCultureIgnoreCase));
                }

                
                return isMatch;

            }

            var filtered = recipes.Where(hasMatch).ToList();
            return filtered.AsReadOnly();
        }

    }
}
