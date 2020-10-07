using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddRecipesServices(this IServiceCollection serviceCollection, string userName, string repoName, string pathInRepo, string token = null)
        {
            return serviceCollection.AddRecipesProvider(userName, repoName, pathInRepo, token);
        }

        static IServiceCollection AddRecipesProvider(this IServiceCollection serviceCollection, string userName, string repoName, string pathInRepo, string token = null)
        {
            var implementationfactrory = new Func<IServiceProvider, IRecipesProvider>((serviceProvider) => new GitHubRecipesProvider(userName, repoName, pathInRepo, token));
            return serviceCollection.AddScoped(implementationfactrory);
        }
    }
}
