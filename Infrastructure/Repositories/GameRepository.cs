using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly ILoggerService _loggerService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpContextAccessor _httpContext;

        public GameRepository(
                        HttpClient httpClient, 
                        IConfiguration configuration, 
                        ILoggerService loggerService, 
                        IHttpContextAccessor httpContextAccessor,
                        IServiceScopeFactory scopeFactory)
        {
            _httpClient = httpClient;
            _endpoint = configuration["GamesAPI:EndPoint"];
            _subscriptionKey = configuration["GamesAPI:OcpApimSubscriptionKey"];
            _authorizationToken = configuration["GamesAPI:Authorization"];
            _loggerService = loggerService;
            _httpContext = httpContextAccessor;
            _scopeFactory = scopeFactory;
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
                #pragma warning disable CS8603 // Possible null reference return.
                return JsonSerializer.Deserialize<Game>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                #pragma warning restore CS8603 // Possible null reference return.
            }

            using var scope = _scopeFactory.CreateScope();
            var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService>();
            loggerService.LogTraceAsync(new Trace()
            {
                LogId = _httpContext.HttpContext?.Items["RequestId"] as Guid?,
                Level = LogLevel.Debug,
                Message = "Infra.GameRepository.GetGameById",
                StackTrace = string.Format("StatusCode = {0} - Content = {1} ", response.StatusCode, response.Content.ReadAsStringAsync().Result)
            });

            return null;
        }
    }
}
