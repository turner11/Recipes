using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public override string ToString()
        {
            return this.Title;
        }

    }


    public class Label
    {
        public string Category { get; }
        public string Title { get; }
        public Label(string category, string title)
        {
            Category = category;
            Title = title;
        }

        public override string ToString()
        {
            return $"Label(category=\"{Category}\", title=\"{Title})\"";
        }

    }
}
