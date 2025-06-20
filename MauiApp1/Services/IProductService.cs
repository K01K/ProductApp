﻿using ProductApp.Models;

namespace ProductApp.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
    }
}