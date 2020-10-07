using Microsoft.AspNetCore.Components;
using Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Components.Recipes
{
    public class RecipeCollectionController : ComponentBase
    {
        [Parameter]
        public IReadOnlyList<IRecipe> Recipes { get; set; }

        [Parameter]
        public EventCallback<IRecipe> RecipeSelected { get; set; }

        protected async Task RecipeClicked(IRecipe recipe)
        {
            await this.RecipeSelected.InvokeAsync(recipe);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
