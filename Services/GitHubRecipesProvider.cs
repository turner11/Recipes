using Octokit;
using Octokit.Reactive;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace Services
{
    public class GitHubRecipesProvider : IRecipesProvider
    {
        public string RepoName { get; }
        public string UserName { get; }
        public string PathInRepo { get; }

        private GitHubClient _github;
        IReadOnlyList<IRecipe> _cache = null;
        public GitHubRecipesProvider(string userName, string repoName, string pathInRepo, string token = null)
        {
            this.UserName = userName;
            this.RepoName = repoName;
            this.PathInRepo = pathInRepo;// $"{this.RepoName}/{pathInRepo}";
            this._github = new GitHubClient(new ProductHeaderValue(repoName));
            if (!String.IsNullOrEmpty(token))
                this._github.Credentials = new Credentials(token);
        }


        public async Task<IReadOnlyList<IRecipe>> GetRecipes()
        {
            if ((this._cache?.Count ?? 0) == 0)
            {
                try
                {

                IReadOnlyList<RepositoryContent> mdNames = await this._github.Repository.Content.GetAllContents(this.UserName, this.RepoName, this.PathInRepo);

                const string suffix = ".md";
                var tasks = mdNames.Select(item => item.Name)
                                    .Where(item => item.ToLowerInvariant().EndsWith(suffix))
                                    .Select(mdName => this._github.Repository.Content.GetAllContents(this.UserName, this.RepoName, $"{this.PathInRepo}/{mdName}"));
                //.Select(mdName => (Name:mdName, Task:this._github.Repository.Content.GetAllContents(this.UserName, this.RepoName, $"{this.PathInRepo}/{mdName}")));

                var mdContents = await Task<string>.WhenAll(tasks);
                var mdContent = mdContents.Select(cs => cs.FirstOrDefault()?.Content?.Trim() ?? "");

                var ret = mdNames.Zip(mdContent, (name, content) => new Recipe(name.Name.Replace(suffix, ""), content)).Where(r => !String.IsNullOrWhiteSpace(r.Instructions))
                        .Cast<IRecipe>()
                        .ToList();

                this._cache = ret.AsReadOnly();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());


                }
            }

            return this._cache;
        }

    }
}
