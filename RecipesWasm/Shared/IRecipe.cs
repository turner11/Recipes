using System;

namespace RecipesWasm.Shared
{
    public interface IRecipe
    {
        string Title { get; }
        string Instructions { get; }
    }
}
