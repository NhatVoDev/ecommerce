﻿using ecommerce.Middleware;
using ecommerce.Models;
using ecommerce.Repository.Interface;

namespace ecommerce.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IRepositoryBase<Category> _repositoryBase;
        public CategoryRepository(IRepositoryBase<Category> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public void AddCategory(Category category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
                CreatedAt = category.CreatedAt
            };
            try
            {
                _repositoryBase.Create(newCategory);
            }
            catch (Exception ex)
            {
                throw new CustomException("Category not added", 500);
            }
        }

        public async void DeleteCategories(List<int> ids)
        {
            try 
            {
                var categories = await _repositoryBase.FindByConditionAsync(c => ids.Contains(c.CategoryId));
                _repositoryBase.DeleteRange(categories);
            }
            catch (Exception ex)
            {
                throw new CustomException("Categories not deleted", 500);
            }
        }


        public void DeleteCategory(Category category)
        {
            try
            {
                _repositoryBase.Delete(category);
            }
            catch (Exception ex)
            {
                throw new CustomException("Category not deleted", 500);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _repositoryBase.FindAllAsync();
            if (categories == null)
            {
                throw new CustomException("No Category found", 404);
            }
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _repositoryBase.FindByIdAsync(id);
            if (category == null)
            {
                throw new CustomException("Category not found", 404);
            }
            return category;
        }

        public async Task<Category> GetCategoryBySlugAsync(string slug)
        {
            var category = await _repositoryBase.FindByConditionAsync(c => c.Slug == slug);
            if (category == null)
            {
                throw new CustomException("Category not found", 404);
            }
            return category.FirstOrDefault() ?? new Category();
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            var categoryToUpdate = await _repositoryBase.FindByIdAsync(id);
            if (categoryToUpdate == null)
            {
                throw new CustomException("Category not found", 404);
            }
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Slug = category.Slug;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.UpdatedAt = category.UpdatedAt;
            _repositoryBase.Update(categoryToUpdate);
        }
    }
}
