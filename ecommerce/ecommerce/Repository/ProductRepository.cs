﻿using ecommerce.Context;
using ecommerce.DTO;
using ecommerce.Middleware;
using ecommerce.Models;
using ecommerce.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryBase<Product> _repositoryBase;
        private readonly EcommerceContext _context;
        public ProductRepository(IRepositoryBase<Product> repositoryBase, EcommerceContext context)
        {
            _repositoryBase = repositoryBase;
            _context = context;
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
                CreatedAt = product.CreatedAt,
                Image = product.Image,
                Gallery = product.Gallery,
                Popular = product.Popular,
                InventoryCount = product.InventoryCount,
                PopularText = product.PopularText,
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
            var products = await _context.Products.Include(u => u.Category).ToListAsync();
            if (products == null)
            {
                throw new CustomException("No Product found", 404);
            }
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.Include(u => u.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                throw new CustomException("Product not found", 404);
            }
            return product;
        }

        public async Task<Product> GetProductBySlugAsync(string slug)
        {
            var product = await _context.Products.Include(u => u.Category).FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null)
            {
                throw new CustomException("Product not found", 404);
            }
            return product;

        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _context.Products.Include(u=>u.Category).Where(p => p.CategoryId == categoryId).ToListAsync();
            if (products == null)
            {
                throw new CustomException("No Product found", 404);
            }
            return products;
        }

        public async Task DeleteProducts(List<int> ids)
        {
            var products = await _context.Products.Where(x => ids.Contains(x.ProductId)).ToListAsync();
            _context.Products.RemoveRange(products);
        }


        public async Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchDto searchDTO)
        {
            var products = await _context.Products.Include(u=>u.Category).Where(p =>
                (string.IsNullOrEmpty(searchDTO.Name) || p.Name.Contains(searchDTO.Name)) &&
                (searchDTO.MinPrice == 0 || p.Price >= searchDTO.MinPrice) &&
                (searchDTO.MaxPrice == 0 || p.Price <= searchDTO.MaxPrice) &&
                (searchDTO.CategoryId == 0 || p.CategoryId == searchDTO.CategoryId)).ToListAsync();

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
            productExist.Image = product.Image;
            productExist.Gallery = product.Gallery;
            productExist.Popular = product.Popular;
            productExist.InventoryCount = product.InventoryCount;
            productExist.PopularText = product.PopularText;
            _repositoryBase.Update(productExist);

        }
    }
}
