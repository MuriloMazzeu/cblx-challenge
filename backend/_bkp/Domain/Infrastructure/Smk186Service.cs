using CblxChallenge.Domain.ViewModels;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Infrastructure
{
    public class Smk186Service : ISmk186Service
    {
        public Smk186Service(IHttpClientFactory factory)
        {
            Http = factory.CreateClient();
        }

        private HttpClient Http { get; }

        public async Task<Smk186Result> GetMineralsAsync(string period)
        {
            var month = period.Substring(5, 1);
            var year = period.Substring(0, 4);
            var week = period.Substring(7, 1);

            var builder = new UriBuilder()
            {
                Scheme = "https",
                Host = "fuct-smk186-code-challenge.cblx.com.br",
                Path = "minerais",
                Query = $"mes={month}&ano={year}&semana={week}"
            };

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            var response = await Http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Smk186Result>(json);
        }
    }
}
