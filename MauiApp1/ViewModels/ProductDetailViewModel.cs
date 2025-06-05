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
                SetProperty(ref _productId, value);
                _ = LoadProductAsync();
            }
        }

        public ProductDetailViewModel(IProductService productService)
        {
            _productService = productService;
        }

        private async Task LoadProductAsync()
        {
            if (ProductId <= 0 || IsBusy)
                return;

            IsBusy = true;

            try
            {
                Product = await _productService.GetProductByIdAsync(ProductId);

                if (Product == null)
                {
                    await Shell.Current.DisplayAlert("Błąd", "Nie znaleziono produktu", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się załadować szczegółów produktu", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}