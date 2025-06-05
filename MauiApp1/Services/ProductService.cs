using System.Text.Json;
using ProductApp.Models;

namespace ProductApp.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _baseUrl = "https://fakestoreapi.com";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{_baseUrl}/products");
                var apiProducts = JsonSerializer.Deserialize<List<ApiProduct>>(response, _jsonOptions);

                return apiProducts?.Select(MapToProduct).ToList() ?? new List<Product>();
            }
            catch (Exception ex)
            {
                // W rzeczywistej aplikacji: logowanie błędu
                return new List<Product>();
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{_baseUrl}/products/{id}");
                var apiProduct = JsonSerializer.Deserialize<ApiProduct>(response, _jsonOptions);

                return apiProduct != null ? MapToProduct(apiProduct) : null;
            }
            catch (Exception ex)
            {
                // W rzeczywistej aplikacji: logowanie błędu
                return null;
            }
        }

        private Product MapToProduct(ApiProduct apiProduct)
        {
            return new Product
            {
                Id = apiProduct.Id,
                Name = apiProduct.Title,
                Price = apiProduct.Price,
                Description = apiProduct.Description,
                Stock = 10, // API nie zwraca stock, więc używamy wartości domyślnej
                ImageUrl = apiProduct.Image,
                Category = apiProduct.Category
            };
        }

        // Klasa pomocnicza do deserializacji JSON z API
        private class ApiProduct
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