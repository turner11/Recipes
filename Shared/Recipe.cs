using System;

namespace Common
{
    public class Recipe : IRecipe
    {
        

        public string Title { get; }
        public string Instructions { get; }

        public Recipe(string title, string instructions)
        {
            this.Title = title;
            this.Instructions = instructions;
        }

        public override string ToString()
        {
            return this.Title;
        }

    }
}
