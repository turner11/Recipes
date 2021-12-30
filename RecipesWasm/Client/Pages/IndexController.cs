﻿using Components.Recipes;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWasm.Client.Pages
{
    public class IndexController:ComponentBase
    {
        [Inject]
        Services.IRecipesProvider RecipesProvider { get; set; }

        [Inject]
        IMatToaster Toaster { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }
        protected IReadOnlyList<RecipeViewModel> Recipes { get; private set; }

        protected RecipeCollection RecipeCollection { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                this.Toaster.Add("Getting recipes", MatToastType.Info); ;
                try
                {
                    var recipes = await this.RecipesProvider.GetRecipes();
                    this.Recipes = recipes.Select(r => new RecipeViewModel(r)).ToList().AsReadOnly();
                    this.Toaster.Clear();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    this.Toaster.Add(ex.Message, MatToastType.Danger, title: "Error getting Recipes"); ;
                }
               
                await this.InvokeAsync(this.StateHasChanged);
            }
        }

        protected void HandleRecipeSelected(RecipeViewModel recipe)
        {
            this.NavigationManager.NavigateTo("Recipe/"+recipe.Title);
        }
    }
}
