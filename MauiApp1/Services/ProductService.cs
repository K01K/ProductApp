using System.Text.Json;
using ProductApp.Models;
using Microsoft.Extensions.Logging;

namespace ProductApp.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<ProductService>? _logger;
        private readonly string _baseUrl = "https://fakestoreapi.com";

        public ProductService(HttpClient httpClient, ILogger<ProductService>? logger = null)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                _logger?.LogInformation("Pobieranie listy produktów");

                var response = await _httpClient.GetStringAsync($"{_baseUrl}/products");
                var apiProducts = JsonSerializer.Deserialize<List<ApiProduct>>(response, _jsonOptions);

                var products = apiProducts?.Select(MapToProduct).ToList() ?? new List<Product>();

                _logger?.LogInformation("Pobrano {Count} produktów", products.Count);
                return products;
            }
            catch (HttpRequestException httpEx)
            {
                _logger?.LogError(httpEx, "Błąd połączenia podczas pobierania produktów");
                throw new InvalidOperationException("Nie można połączyć się z serwerem. Sprawdź połączenie internetowe.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                _logger?.LogError(jsonEx, "Błąd deserializacji danych produktów");
                throw new InvalidOperationException("Otrzymano nieprawidłowe dane z serwera.", jsonEx);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Nieoczekiwany błąd podczas pobierania produktów");
                throw;
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger?.LogWarning("Próba pobrania produktu z nieprawidłowym ID: {Id}", id);
                return null;
            }

            try
            {
                _logger?.LogInformation("Pobieranie produktu o ID: {Id}", id);

                var response = await _httpClient.GetStringAsync($"{_baseUrl}/products/{id}");
                var apiProduct = JsonSerializer.Deserialize<ApiProduct>(response, _jsonOptions);

                var product = apiProduct != null ? MapToProduct(apiProduct) : null;

                if (product != null)
                    _logger?.LogInformation("Pobrano produkt: {ProductName}", product.Name);
                else
                    _logger?.LogWarning("Nie znaleziono produktu o ID: {Id}", id);

                return product;
            }
            catch (HttpRequestException httpEx)
            {
                _logger?.LogError(httpEx, "Błąd połączenia podczas pobierania produktu {Id}", id);
                throw new InvalidOperationException("Nie można połączyć się z serwerem. Sprawdź połączenie internetowe.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                _logger?.LogError(jsonEx, "Błąd deserializacji danych produktu {Id}", id);
                throw new InvalidOperationException("Otrzymano nieprawidłowe dane z serwera.", jsonEx);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Nieoczekiwany błąd podczas pobierania produktu {Id}", id);
                throw;
            }
        }

        private static Product MapToProduct(ApiProduct apiProduct)
        {
            return new Product
            {
                Id = apiProduct.Id,
                Name = string.IsNullOrWhiteSpace(apiProduct.Title) ? "Produkt bez nazwy" : apiProduct.Title,
                Price = Math.Max(0, apiProduct.Price),
                Description = apiProduct.Description ?? string.Empty,
                Stock = Random.Shared.Next(0, 50), 
                ImageUrl = apiProduct.Image ?? string.Empty,
                Category = apiProduct.Category ?? "Inne"
            };
        }
        private sealed class ApiProduct
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public decimal Price { get; set; }
            public string? Description { get; set; }
            public string? Category { get; set; }
            public string? Image { get; set; }
        }
    }
}