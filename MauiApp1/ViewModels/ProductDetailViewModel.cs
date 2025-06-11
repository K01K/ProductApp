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
            private set => SetProperty(ref _product, value);
        }

        public int ProductId
        {
            get => _productId;
            set
            {
                if (SetProperty(ref _productId, value))
                {
                    _ = LoadProductAsync();
                }
            }
        }

        public ProductDetailViewModel(IProductService productService)
        {
            _productService = productService;
            Title = "Szczegóły Produktu";
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
                    await ShowErrorAsync("Nie znaleziono produktu");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message);
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}