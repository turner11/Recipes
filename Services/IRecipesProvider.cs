using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IRecipesProvider
    {
        Task<IReadOnlyList<IRecipe>> GetRecipes();
        Task<IReadOnlyList<string>> GetRecipesNames();
    }
}
