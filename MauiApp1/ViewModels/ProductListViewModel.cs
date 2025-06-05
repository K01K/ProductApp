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

        public ObservableCollection<Product> Products { get; }
        public ICommand LoadProductsCommand { get; }
        public ICommand ProductSelectedCommand { get; }

        public ProductListViewModel(IProductService productService)
        {
            _productService = productService;
            Products = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(async () => await LoadProductsAsync());
            ProductSelectedCommand = new Command<Product>(async (product) => await OnProductSelected(product));
        }

        public async Task InitializeAsync()
        {
            await LoadProductsAsync();
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
                // W rzeczywistej aplikacji: pokazanie komunikatu błędu użytkownikowi
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się załadować produktów", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnProductSelected(Product product)
        {
            if (product == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?id={product.Id}");
        }
    }
}