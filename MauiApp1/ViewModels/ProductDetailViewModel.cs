using ProductApp.Models;
using ProductApp.Services;

namespace ProductApp.ViewModels
{
    [QueryProperty(nameof(ProductId), "id")]
    public class ProductDetailViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private Product? _product;
        private int _productId;

        public Product? Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public int ProductId
        {
            get => _productId;
            set
            {
                if (SetProperty(ref _productId, value))
                {
                    _ = Task.Run(LoadProductAsync);
                }
            }
        }
        public bool HasProduct => Product != null;
        public string ProductName => Product?.Name ?? "Ładowanie...";
        public string ProductDescription => Product?.Description ?? string.Empty;

        public ProductDetailViewModel(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            Title = "Szczegóły Produktu";
        }

        private async Task LoadProductAsync()
        {
            if (ProductId <= 0 || IsBusy)
                return;

            IsBusy = true;

            await ExecuteSafeAsync(async () =>
            {
                var product = await _productService.GetProductByIdAsync(ProductId);

                if (product != null)
                {
                    Product = product;
                    Title = $"Szczegóły: {product.Name}";
                    OnPropertyChanged(nameof(HasProduct));
                    OnPropertyChanged(nameof(ProductName));
                    OnPropertyChanged(nameof(ProductDescription));
                }
                else
                {
                    await HandleErrorAsync("Nie znaleziono produktu o podanym identyfikatorze");
                    if (Shell.Current.Navigation.NavigationStack.Count > 1)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                }

            }, "Nie udało się załadować szczegółów produktu");

            IsBusy = false;
        }
        public async Task RefreshProductAsync()
        {
            if (ProductId > 0)
            {
                await LoadProductAsync();
            }
        }
    }
}