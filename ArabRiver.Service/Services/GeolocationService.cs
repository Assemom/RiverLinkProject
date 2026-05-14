using ArabRiver.Service.Helpers;
using ArabRiver.Service.Interfaces;
using System.Net.Http.Json;

namespace ArabRiver.Service.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;

        public GeolocationService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        public async Task<(string Country,
            string CountryCode,
            bool IsEgypt)>
            GetLocationAsync(string ipAddress)
        {
            try
            {
                var url =
                    $"http://ip-api.com/json/{ipAddress}";

                var response =
                    await _httpClient
                        .GetFromJsonAsync<
                            GeolocationResponse>(url);

                if (response is null)
                {
                    return ("Unknown", "UN", false);
                }

                var isEgypt =
                    response.CountryCode
                        .Equals("EG",
                            StringComparison
                                .OrdinalIgnoreCase);

                return (
                    response.Country,
                    response.CountryCode,
                    isEgypt);
            }
            catch
            {
                return ("Unknown", "UN", false);
            }
        }
    }
}
