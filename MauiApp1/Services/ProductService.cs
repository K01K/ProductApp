using System.Text.Json;
using ProductApp.Models;

namespace ProductApp.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string BaseUrl = "https://fakestoreapi.com";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{BaseUrl}/products");
                var apiProducts = JsonSerializer.Deserialize<List<ApiProduct>>(response, _jsonOptions);

                return apiProducts?.Select(MapToProduct).ToList() ?? new List<Product>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP Error: {ex.Message}");
                throw new InvalidOperationException("Nie udało się pobrać danych z serwera");
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Timeout Error: {ex.Message}");
                throw new InvalidOperationException("Przekroczono czas oczekiwania na odpowiedź");
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON Error: {ex.Message}");
                throw new InvalidOperationException("Błąd w formacie danych z serwera");
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{BaseUrl}/products/{id}");
                var apiProduct = JsonSerializer.Deserialize<ApiProduct>(response, _jsonOptions);

                return apiProduct != null ? MapToProduct(apiProduct) : null;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP Error: {ex.Message}");
                throw new InvalidOperationException("Nie udało się pobrać szczegółów produktu");
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Timeout Error: {ex.Message}");
                throw new InvalidOperationException("Przekroczono czas oczekiwania na odpowiedź");
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON Error: {ex.Message}");
                throw new InvalidOperationException("Błąd w formacie danych z serwera");
            }
        }

        private static Product MapToProduct(ApiProduct apiProduct)
        {
            return new Product
            {
                Id = apiProduct.Id,
                Name = apiProduct.Title,
                Price = apiProduct.Price,
                Description = apiProduct.Description,
                Stock = Random.Shared.Next(5, 50),
                ImageUrl = apiProduct.Image,
                Category = apiProduct.Category
            };
        }
        private sealed class ApiProduct
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Description { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string Image { get; set; } = string.Empty;
        }
    }
}