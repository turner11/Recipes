using Components.Recipes;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using RecipesWasm.Client.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWasm.Client.Pages
{
    public partial class Index
    {    

        [Inject]
        IMatToaster Toaster { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        CascadingAppState appState { get; set; }
        protected IReadOnlyList<RecipeViewModel> Recipes => appState.Recipes;

        protected RecipeCollection RecipeCollection { get; set; }
      
        protected void HandleRecipeSelected(RecipeViewModel recipe)
        {
            this.NavigationManager.NavigateTo("Recipe/"+recipe.Title);
        }
    }
}
