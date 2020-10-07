using Common;
using Components.Recipes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Pages
{
    public class IndexController:ComponentBase
    {
        [Inject]
        Services.IRecipesProvider RecipesProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }
        protected IReadOnlyList<RecipeViewModel> Recipes { get; private set; }

        protected Components.Recipes.RecipeCollection RecipeCollection { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var recipes = await this.RecipesProvider.GetRecipes();
                this.Recipes = recipes.Select(r => new RecipeViewModel(r)).ToList().AsReadOnly();
                await this.InvokeAsync(this.StateHasChanged);
            }
        }

        protected void HandleRecipeSelected(RecipeViewModel recipe)
        {
            this.NavigationManager.NavigateTo("Recipe/"+recipe.Title);
        }
    }
}
