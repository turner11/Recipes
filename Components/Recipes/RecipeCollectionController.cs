﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Components.Recipes
{
    public class RecipeCollectionController : ComponentBase
    {
        [Parameter]
        public IReadOnlyList<RecipeViewModel> Recipes { get; set; }

        [Parameter]
        public EventCallback<RecipeViewModel> RecipeSelected { get; set; }

        protected async Task RecipeClicked(RecipeViewModel recipe)
        {
            await this.RecipeSelected.InvokeAsync(recipe);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
