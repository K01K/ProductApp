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

        public ObservableCollection<Product> Products { get; }
        public ICommand LoadProductsCommand { get; }
        public ICommand ProductSelectedCommand { get; }

        public ProductListViewModel(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));

            Products = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(async () => await LoadProductsAsync(), () => !IsBusy);
            ProductSelectedCommand = new Command<Product>(async (product) => await OnProductSelectedAsync(product));

            Title = "Lista Produktów";
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized || IsBusy)
                return;

            await LoadProductsAsync();
            _isInitialized = true;
        }

        private async Task LoadProductsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            await ExecuteSafeAsync(async () =>
            {
                var products = await _productService.GetProductsAsync();
                Products.Clear();

                foreach (var product in products.OrderBy(p => p.Name))
                {
                    Products.Add(product);
                }

                ((Command)LoadProductsCommand).ChangeCanExecute();

            }, "Nie udało się załadować produktów. Sprawdź połączenie internetowe i spróbuj ponownie.");

            IsBusy = false;
            ((Command)LoadProductsCommand).ChangeCanExecute();
        }

        private async Task OnProductSelectedAsync(Product? product)
        {
            if (product == null)
                return;

            try
            {
                await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?id={product.Id}");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Nie udało się otworzyć szczegółów produktu", ex);
            }
        }

        public async Task RefreshAsync()
        {
            _isInitialized = false;
            await InitializeAsync();
        }
    }
}