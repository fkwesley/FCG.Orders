using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        private readonly string _subscriptionKey;
        private readonly string _authorizationToken;

        public GameRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _endpoint = configuration["GamesAPI:EndPoint"];
            _subscriptionKey = configuration["GamesAPI:OcpApimSubscriptionKey"];
            _authorizationToken = configuration["GamesAPI:Authorization"];
        }

        public Game GetGameById(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint}/{id}");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationToken);

            var response = _httpClient.Send(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<Game>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }
    }
}
