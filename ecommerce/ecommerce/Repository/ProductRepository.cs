﻿using ecommerce.DTO;
using ecommerce.Middleware;
using ecommerce.Models;
using ecommerce.Repository.Interface;

namespace ecommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryBase<Product> _repositoryBase;
        public ProductRepository(IRepositoryBase<Product> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public void AddProduct(Product product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Slug = product.Slug,
            };
            _repositoryBase.Create(newProduct);
        }

        public void DeleteProduct(Product? product)
        {
            if (product == null)
            {
                throw new CustomException("No Product found", 404);
            }
            _repositoryBase.Delete(product);

        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _repositoryBase.FindAllAsync();
            if (products == null)
            {
                throw new CustomException("No Product found", 404);
            }
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _repositoryBase.FindByIdAsync(id);
            if (product == null)
            {
                throw new CustomException("Product not found", 404);
            }
            return product;
        }

        public async Task<Product> GetProductBySlugAsync(string slug)
        {
            var product = await _repositoryBase.FindByConditionAsync(p => p.Slug == slug);
            if (product == null)
            {
                throw new CustomException("Product not found", 404);
            }
            return product.FirstOrDefault();

        }

        public Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = _repositoryBase.FindByConditionAsync(p => p.CategoryId == categoryId);
            if (products == null)
            {
                throw new CustomException("No Product found", 404);
            }
            return products;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchDto searchDTO)
        {
            var products = await _repositoryBase.FindByConditionAsync(p =>
                (string.IsNullOrEmpty(searchDTO.Name) || p.Name.Contains(searchDTO.Name)) &&
                (searchDTO.MinPrice == 0 || p.Price >= searchDTO.MinPrice) &&
                (searchDTO.MaxPrice == 0 || p.Price <= searchDTO.MaxPrice) &&
                (searchDTO.CategoryId == 0 || p.CategoryId == searchDTO.CategoryId));

            if (products == null)
            {
                throw new CustomException("No Product found", 404);
            }

            return products;
        }

        public void UpdateProduct(Product product, Product productExist)
        {
            productExist.Name = product.Name;
            productExist.Description = product.Description;
            productExist.Price = product.Price;
            productExist.UpdatedAt = DateTime.UtcNow;
            productExist.Slug = product.Slug;
            _repositoryBase.Update(productExist);

        }
    }
}
