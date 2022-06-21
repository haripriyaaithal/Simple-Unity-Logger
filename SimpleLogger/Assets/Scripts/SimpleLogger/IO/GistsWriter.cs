using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SimpleLogger.Data;
using UnityEngine;

namespace SimpleLogger.IO
{
    public class GistsWriter : BaseLogWriter
    {
        private string _authToken;
        private const string CREATE_GIST_API_ENDPOINT = "https://api.github.com/gists";

        public GistsWriter(string personalAccessToken)
        {
            _authToken = personalAccessToken;
        }

        public override void WriteLogs(IEnumerable<LogData> logs, string fileName, string description, Action<string, string> onComplete)
        {
            Task.Run(async () =>
            {
                using var httpClient = new HttpClient();
                SetRequestHeaders(httpClient);

                var logString = ProcessLogs(logs);
                var requestJson = GenerateRequestJson(fileName, description, logString);
                var data = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var respone = await httpClient.PostAsync(CREATE_GIST_API_ENDPOINT, data);
                if (respone.IsSuccessStatusCode)
                {
                    var gistResponse = await respone.Content.ReadAsStringAsync();
                    var response = JsonUtility.FromJson<GistsResponseObject>(gistResponse);
                    onComplete?.Invoke(response.html_url, string.Empty);
                }
                else
                    onComplete?.Invoke(string.Empty, respone.StatusCode.ToString());
            });
        }

        private void SetRequestHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github.v3+json");
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("unity");
            httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authToken);
        }

        private string GenerateRequestJson(string fileName, string description, string logString)
        {
            dynamic result = new DynamicJSON();
            result.description = description;
            result.@public = true;
            result.files = new { };
            foreach (var fileContent in new[] { Tuple.Create($"{fileName}", logString) })
                result.files[fileContent.Item1] = new { content = fileContent.Item2 };

            return result.ToString();
        }
    }
}