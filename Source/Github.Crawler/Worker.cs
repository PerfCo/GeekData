using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Contracts.Github;
using NLog;
using RestSharp;
using RestSharp.Authenticators;

namespace Github.Crawler
{
    public sealed class Worker
    {
        private const int ItemsPerPage = 100; //100 - max page size
        private const int ItemsTotal = 100; //1000 - max search results from github
        private const string Login = "firealkazar";
        private const string Password = "git_vs_svn1";
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public List<GithubRepositoryInfo> GetRepositories(string searchValue, string language)
        {
            List<GithubRepositoryInfo> repositories = GetRepositoriesBasicInfo(searchValue, language);
            FillReadme(repositories);
            return repositories;
        }

        private static string BuildQuery(string searchValue, string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return searchValue;
            }

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return $"language:{language}";
            }

            return $"{searchValue}+language:{language}";
        }

        private static void FillReadme(List<GithubRepositoryInfo> repositories)
        {
            var restClient = new RestClient(@"https://api.github.com/repos")
            {
                Authenticator = new HttpBasicAuthenticator(Login, Password)
            };

            foreach (GithubRepositoryInfo repository in repositories)
            {
                string repoPath = new Uri(repository.HtmlUrl).LocalPath;
                string readmePath = repoPath + @"/readme";
                IRestResponse<GithubContentResponse> response = restClient.Get<GithubContentResponse>(new RestRequest(readmePath));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    repository.Readme = Encoding.UTF8.GetString(Convert.FromBase64String(response.Data.Content));
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    repository.Readme = string.Empty;
                    _logger.Warn($"README for {repoPath} repo not found ");
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.Warn("API rate limit exceeded, continue in an hour or change credentials");
                    break;
                }
                else
                {
                    _logger.Warn($"Can't get README for {repoPath} repo - {response.StatusCode}");
                }
            }
        }

        private static List<GithubRepositoryInfo> GetRepositoriesBasicInfo(string searchValue, string language)
        {
            var pageCount = (int)Math.Ceiling((double)ItemsTotal / ItemsPerPage);

            var result = new List<GithubRepositoryInfo>();

            for (var pageNumber = 1; pageNumber <= pageCount; pageNumber++)
            {
                List<GithubRepositoryInfo> repositoriesByPage = GetRepositoriesByPageNumber(searchValue, language, pageNumber);
                if (repositoriesByPage == null)
                {
                    break;
                }
                result.AddRange(repositoriesByPage);
            }

            return result;
        }

        private static List<GithubRepositoryInfo> GetRepositoriesByPageNumber(string searchValue, string language, int pageNumber)
        {
            var restClient = new RestClient(@"https://api.github.com/search/repositories")
            {
                Authenticator = new HttpBasicAuthenticator(Login, Password)
            };
            string queryValue = BuildQuery(searchValue, language);
            restClient.AddDefaultParameter("q", queryValue);
            restClient.AddDefaultParameter("sort", "stars");
            restClient.AddDefaultParameter("order", "desc");
            restClient.AddDefaultParameter("page", pageNumber);
            restClient.AddDefaultParameter("per_page", ItemsPerPage);

            IRestResponse<GithubRepositoryResponse> response = restClient.Get<GithubRepositoryResponse>(new RestRequest());
            return response.Data.Items;
        }


        private sealed class GithubContentResponse
        {
            public string Content { get; set; }
        }


        private sealed class GithubRepositoryResponse
        {
            public int TotalCount { get; set; }
            public List<GithubRepositoryInfo> Items { get; set; }
        }
    }
}
