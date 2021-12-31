using Components.Recipes;
using Microsoft.AspNetCore.Components;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWasm.Client.Pages
{
    public partial class Recipe
    {
        [Inject]
        IRecipesProvider RecipesProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string RecipeName { get; set; }

        protected string Title => this.RecipeVM?.Title;

        protected string directionStyle => (this.RecipeVM?.Instructions ?? "").GetHtmlInnerText().GuessDirectionStyle();
        public RecipeViewModel RecipeVM { get; private set; }

        public MarkupString Instructions => this.RecipeVM?.InstructionsHtml ?? new MarkupString();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (String.IsNullOrWhiteSpace(RecipeName))
            {
                this.RecipeVM = null;
                return;
            }
            var currTitle = this.RecipeVM?.Title ?? "";
            var getRecipe = !currTitle.Equals(this.RecipeName ?? "", StringComparison.InvariantCultureIgnoreCase);
            if (getRecipe)
            {
                var recipe = await this.RecipesProvider.GetRecipe(RecipeName);
                this.RecipeVM = new RecipeViewModel(recipe);
                
                await this.InvokeAsync(StateHasChanged);

            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                if (this.RecipeVM == null)
                {
                    this.NavigationManager.NavigateTo("/");
                }
            }
        }



    }
}
