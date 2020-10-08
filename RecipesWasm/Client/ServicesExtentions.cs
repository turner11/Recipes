using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWasm.Client
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddRecipesServices(this IServiceCollection serviceCollection, string userName=null, string repoName = null, string pathInRepo = null, string token = null)
        {
            return serviceCollection.AddRecipesProvider(userName, repoName, pathInRepo, token);
        }

        static IServiceCollection AddRecipesProvider(this IServiceCollection serviceCollection, string userName, string repoName, string pathInRepo, string token = null)
        {
            var implementationfactrory = new Func<IServiceProvider, IRecipesProvider>((serviceProvider) =>
            {
                if (new[] { userName, repoName, pathInRepo, token }.Any(String.IsNullOrWhiteSpace))
                {
                    var Configuration = serviceProvider.GetService<IConfiguration>();

                    var GithubSection = Configuration.GetSection("Github");
                    userName ??= GithubSection.GetValue<string>("User");
                    repoName ??= GithubSection.GetValue<string>("Repo");
                    pathInRepo ??= GithubSection.GetValue<string>("Path");
                    token ??= GithubSection.GetValue<string>("token");
                }

                return new GitHubRecipesProvider(userName, repoName, pathInRepo, token);
            }); 
            return serviceCollection.AddScoped(serviceProvider=> implementationfactrory(serviceProvider));
        }
    }
}
