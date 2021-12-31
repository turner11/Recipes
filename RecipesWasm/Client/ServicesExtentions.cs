using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;

namespace RecipesWasm.Client
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddRecipesServices(this IServiceCollection serviceCollection, string userName = null, string repoName = null, string token = null)
        {
            return serviceCollection.AddRecipesProvider(userName, repoName, token)
                                    .AddToaster();


        }

        static IServiceCollection AddToaster(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });
        }

        static IServiceCollection AddRecipesProvider(this IServiceCollection serviceCollection, string userName, string repoName, string token = null)
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;

            //Doing it here using a lambda serves us well for allowing to add this service basaed on configuration in WASM.
            var implementationfactrory = new Func<IServiceProvider, IRecipesProvider>((serviceProvider) =>
            {
                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;


                if (new[] { userName, repoName, token }.Any(String.IsNullOrWhiteSpace))
                {
                    var tpl = serviceProvider.GetRepoInfoFromConfiguration();
                    userName ??= tpl.userName;
                    repoName ??= tpl.repoName;
                    token ??= tpl.token;
                }

                return new GitHubRecipesProvider(userName, repoName, token);
            });
            return serviceCollection.AddScoped(serviceProvider => implementationfactrory(serviceProvider));
        }



        public static (string userName, string repoName, string token, bool isProduction) GetRepoInfoFromConfiguration(this IServiceProvider serviceProvider)
        {
            var Configuration = serviceProvider.GetService<IConfiguration>();
            return Configuration.GetRepoInfo();
        }

        public static (string userName, string repoName, string token, bool isProduction) GetRepoInfo(this IConfiguration Configuration)
        {
            var isProduction = Configuration.GetValue<bool>("IsProduction");
            var GithubSection = Configuration.GetSection("Github");
            var userName = GithubSection.GetValue<string>("User");
            var repoName = GithubSection.GetValue<string>("Repo");
            var token = GithubSection.GetValue<string>("token");

            return (userName, repoName, token, isProduction);
        }
    }
}
