﻿using Components.Recipes;
using Microsoft.AspNetCore.Components;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWasm.Client.Pages
{
    public class RecipeController : ComponentBase
    {
        [Inject]
        IRecipesProvider RecipesProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string RecipeName { get; set; }

        protected string Title => this.Recipe?.Title;

        protected string directionStyle => (this.Recipe?.Instructions ?? "").GetHtmlInnerText().GuessDirectionStyle();
        public RecipeViewModel Recipe { get; private set; }

        public MarkupString Instructions => this.Recipe?.InstructionsHtml ?? new MarkupString();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (String.IsNullOrWhiteSpace(RecipeName))
            {
                this.Recipe = null;
                return;
            }
            var currTitle = this.Recipe?.Title ?? "";
            var getRecipe = !currTitle.Equals(this.RecipeName ?? "", StringComparison.InvariantCultureIgnoreCase);
            if (getRecipe)
            {
                var recipe = await this.RecipesProvider.GetRecipe(RecipeName);
                this.Recipe = new RecipeViewModel(recipe);
                
                await this.InvokeAsync(StateHasChanged);

            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                if (this.Recipe == null)
                {
                    this.NavigationManager.NavigateTo("/");
                }
            }
        }



    }
}
