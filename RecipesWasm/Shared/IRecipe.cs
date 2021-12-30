using System.Collections.ObjectModel;

namespace RecipesWasm.Shared
{
    public interface IRecipe
    {
        string Title { get; }
        string Instructions { get; }
        ReadOnlyCollection<Label> Labels { get; }
    }
}
