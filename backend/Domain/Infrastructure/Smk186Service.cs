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
            var parts = period.Split('-', '_');
            var builder = new UriBuilder()
            {
                Scheme = "https",
                Host = "fuct-smk186-code-challenge.cblx.com.br",
                Path = "minerais",
                Query = $"mes={parts[1]}&ano={parts[0]}&semana={parts[2]}"
            };

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            var response = await Http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Smk186Result>(json);
        }
    }
}
