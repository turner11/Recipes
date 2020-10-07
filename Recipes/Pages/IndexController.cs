using Common;
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
        protected IReadOnlyList<IRecipe> Recipes { get; private set; }

        protected Components.Recipes.RecipeCollection RecipeCollection { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                this.Recipes = await this.RecipesProvider.GetRecipes();
                await this.InvokeAsync(this.StateHasChanged);
            }
        }

        protected void HandleRecipeSelected(IRecipe recipe)
        {

        }
    }
}
