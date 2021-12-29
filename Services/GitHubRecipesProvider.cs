using Octokit;
using Octokit.Reactive;
using RecipesWasm.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace Services
{
    public class GitHubRecipesProvider : IRecipesProvider
    {
        const string RECIPES_LABEL = "recipe";
        public string RepoName { get; }
        public string UserName { get; }
        

        IReadOnlyList<Issue> _issues;

        private GitHubClient _github;
        IReadOnlyList<IRecipe> _cache = null;
        public GitHubRecipesProvider(string userName, string repoName, string token)
        {
            this.UserName = userName;
            this.RepoName = repoName;
            if (String.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token), "Cannot use a null / empthy token");

            this._github = new GitHubClient(new ProductHeaderValue(repoName))
            {
                Credentials = new Credentials(token)                
            };
            
            
        }

        async Task<IReadOnlyList<Issue>> GetRecipesIssues()
        {
            if (this._issues is not null)            
                return this._issues;
            
            var request = new RepositoryIssueRequest
            {
                //Assignee = "none",
                //Milestone = "none",
                Filter = IssueFilter.All,
                //State = ItemStateFilter.Closed
            };
            request.Labels.Add(RECIPES_LABEL);


            IReadOnlyList<Issue> issues = await this._github.Issue.GetAllForRepository(this.UserName, this.RepoName, request);
            this._issues = issues;
            return issues;
        }

        public async Task<IReadOnlyList<string>> GetRecipesNames()
        {
            var recipes = await this.GetRecipes().ConfigureAwait(false);
            var names = recipes.Select(recipe=> recipe.Title).ToList().AsReadOnly();
            return names;
        }


        private IReadOnlyList<RecipesWasm.Shared.Label> GetLabels(Issue issue)
        {
            var splitter = ": ";
            var labels = issue.Labels.Select(lbl => lbl.Name)
                               .Where(title => title.Contains(splitter))
                               .Select(title=> title.Split(splitter, 2, StringSplitOptions.RemoveEmptyEntries))
                               .Select(fregmants => new RecipesWasm.Shared.Label(fregmants[0], fregmants[^1]))
                               .ToList().AsReadOnly();
            return labels;
        }

        public async Task<IReadOnlyList<IRecipe>> GetRecipes()
        {
            if ((this._cache?.Count ?? 0) == 0)
            {
                try
                {
                    var issues = await this.GetRecipesIssues().ConfigureAwait(false);
                    var ret = issues.Select(issue => new Recipe(issue.Title, issue.Body, GetLabels(issue))).Where(r => !String.IsNullOrWhiteSpace(r.Instructions))
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

        public async Task<IRecipe> GetRecipe(string name)
        {
            var recipes = await this.GetRecipes();
            var recipe = recipes.FirstOrDefault(r => r.Title.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return recipe;
        }
    }
}
