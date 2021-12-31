using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace RecipesWasm.Shared
{
    public class Recipe : IRecipe
    {
        public string Title { get; }
        public string Instructions { get; }
        public ReadOnlyCollection<Label> Labels { get; }

        public Recipe(string title, string instructions, IEnumerable<Label> labels = null)
        {
            this.Title = title;
            this.Instructions = instructions;
            this.Labels = (labels ?? new Label[0]).ToList().AsReadOnly();
        }

        public override string ToString() => this.Title;
    }


    public record class Label(string Category, string Value)
    {

        public override string ToString()
        {
            return $"Label(category=\"{Category}\", value=\"{Value})\"";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Category.ToLower(CultureInfo.InvariantCulture).GetHashCode(), this.Value.ToLower(CultureInfo.InvariantCulture).GetHashCode());
        }


    }
}
