namespace ProductApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public string FormattedPrice => $"{Price:C}";
        public string StockDisplay => Stock > 0 ? $"Dostępne: {Stock} szt." : "Brak w magazynie";
        public bool IsInStock => Stock > 0;
    }
}