using System;

namespace Common
{
    public interface IRecipe
    {
        string Title { get; }
        string Instructions { get; }
    }
}
