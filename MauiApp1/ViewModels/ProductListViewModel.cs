using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductApp.Models;
using ProductApp.Services;
using ProductApp.Views;

namespace ProductApp.ViewModels
{
    public class ProductListViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private bool _isInitialized;

        public ObservableCollection<Product> Products { get; } = new();
        public ICommand LoadProductsCommand { get; }
        public ICommand ProductSelectedCommand { get; }

        public ProductListViewModel(IProductService productService)
        {
            _productService = productService;
            Title = "Lista Produktów";
            LoadProductsCommand = new Command(async () => await LoadProductsAsync());
            ProductSelectedCommand = new Command<Product>(async (product) => await OnProductSelectedAsync(product));
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            await LoadProductsAsync();
            _isInitialized = true;
        }

        private async Task LoadProductsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var products = await _productService.GetProductsAsync();

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnProductSelectedAsync(Product? product)
        {
            if (product == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?id={product.Id}");
        }
    }
}