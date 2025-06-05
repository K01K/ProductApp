﻿using ProductApp.ViewModels;

namespace ProductApp.Views
{
    public partial class ProductListPage : ContentPage
    {
        private readonly ProductListViewModel _viewModel;

        public ProductListPage(ProductListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitializeAsync();
        }
    }
}