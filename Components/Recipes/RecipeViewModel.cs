using RecipesWasm.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Components.Recipes
{
    public class RecipeViewModel
    {
        const string defaultImageUrl = "https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/pure-white-1554141426.jpg?crop=1xw:1xh;center,top&resize=480:*";
        public IRecipe Recipe { get; }

        public string Title => this.Recipe.Title;

        public string Instructions => this.Recipe.Instructions;

        public MarkupString InstructionsHtml => GetInstructions(this.Instructions);

        public string ImageUrl { get; }

        public RecipeViewModel(IRecipe recipe)
        {
            Recipe = recipe;
            ImageUrl = this.GetImageUrl(this.Recipe);
        }

        private string GetImageUrl(IRecipe recipe)
        {
            string matchString = Regex.Match(recipe.Instructions, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

            if (String.IsNullOrWhiteSpace(matchString))
            {
                matchString = new Regex(@"!\[.*?\]\(.*?\)")
                        .Matches(recipe.Instructions)
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .FirstOrDefault() ?? "";
                var idxStart = matchString.IndexOf("(") +1 ;
                var idxEnd = matchString.IndexOf(")");
                if (idxStart >= 0 && idxEnd > idxStart)
                {
                    matchString = matchString.Substring(idxStart, idxEnd - idxStart);
                }
            }
            
            
            if (String.IsNullOrWhiteSpace(matchString))
            {
                matchString = defaultImageUrl;
            }
            return matchString;
        }

        MarkupString GetInstructions(string markupString)
        {
            var options = new HeyRed.MarkdownSharp.MarkdownOptions
            {
                AutoHyperlink = true,
                LinkEmails = true,
                QuoteSingleLine = true,
                StrictBoldItalic = true
            };

            var mark = new HeyRed.MarkdownSharp.Markdown(options);
            var str = mark.Transform(markupString);
            var html = new MarkupString(str);
            return html;
        }

    }
}
