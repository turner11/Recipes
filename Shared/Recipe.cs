using System;

namespace Common
{
    public class Recipe : IRecipe
    {
        

        public string Title { get; }
        public string Instructions { get; }

        public string ImageUrl => "https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/pure-white-1554141426.jpg?crop=1xw:1xh;center,top&resize=480:*";

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
