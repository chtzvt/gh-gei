﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoshiftCLI.BbsToGithub.Factories;
using OctoshiftCLI.Services;

namespace OctoshiftCLI.BbsToGithub
{
    public class ReposCsvGeneratorService
    {
        private readonly BbsApi _bbsApi;
        private readonly BbsInspectorServiceFactory _bbsInspectorServiceFactory;

        public ReposCsvGeneratorService(BbsApi bbsApi, BbsInspectorServiceFactory bbsInspectorServiceFactory)
        {
            _bbsApi = bbsApi;
            _bbsInspectorServiceFactory = bbsInspectorServiceFactory;
        }

        public virtual async Task<string> Generate(string bbsServerUrl, string bbsProject, bool minimal = false)
        {
            var inspector = _bbsInspectorServiceFactory.Create(_bbsApi);
            var result = new StringBuilder();

            result.Append("project,repo,url,last-commit-date,compressed-repo-size-in-bytes");
            result.AppendLine(!minimal ? ",is-archived,pr-count" : null);

            var projects = string.IsNullOrEmpty(bbsProject) ? await inspector.GetProjects() : new[] { bbsProject }.ToList();

            foreach (var project in projects)
            {
                foreach (var repo in await inspector.GetRepos(project))
                {
                    var url = $"{bbsServerUrl.TrimEnd('/')}/projects/{project}";
                    // var prCount = !minimal ? await inspector.GetPullRequestCount(project, repo.Name) : 0;
                    // var lastCommitDate = await _bbsApi.GetLastCommitDate(project, repo.Name);

                    // result.Append($"\"{project}\",\"{repo.Name}\",\"{url}\",\"{lastCommitDate:dd-MMM-yyyy hh:mm tt}\",\"{repo.Size:N0}\"");
                    // result.AppendLine(!minimal ? $",\"{repo.Archived}\",{prCount}" : null);
                    // result.AppendLine(!minimal ? $",{prCount}" : null);
                }
            }

            return result.ToString();
        }
    }
}
