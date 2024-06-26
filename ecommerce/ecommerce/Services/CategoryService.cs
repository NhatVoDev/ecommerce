﻿using ecommerce.Context;
using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Models;
using ecommerce.Repository.Interface;
using ecommerce.Services.Interface;
using ecommerce.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EcommerceContext _context;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, EcommerceContext context)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<ApiResponse<int>> AddCategoryAsync(CategoryDto category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
                Description = category.Description,
                Slug = StringHelper.GenerateSlug(category.Name),
                CreatedAt = DateTime.UtcNow
            };
            _categoryRepository.AddCategory(newCategory);
            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>
            {
                Data = newCategory.CategoryId,
                Message = "Category added",
                Status = true
            };
        }

        public async Task<ApiResponse<int>> DeleteCategoriesAsync(List<int> ids)
        {
            try
            {
                _categoryRepository.DeleteCategories(ids);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<int>
                {
                    Data = 0,
                    Message = "Categories deleted",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Data = 0,
                    Message = ex.Message,
                    Status = false
                };
            }
        }


        public async Task<ApiResponse<int>> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = "No Category found",
                    Status = false
                };
            }
            try
            {
                _categoryRepository.DeleteCategory(category);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = "Category deleted",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = ex.Message,
                    Status = false
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<CategoryAllDto>>> GetAllCategoriesAsync(PagingForBlogCategory? paging = null)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(paging);
            var total = await _context.Categories.CountAsync();
            if (categories.Item1 == null)
            {
                return new ApiResponse<IEnumerable<CategoryAllDto>>
                {
                    Data = null,
                    Message = "No Category found",
                    Status = false
                };
            }
            var result = new ApiResponse<IEnumerable<CategoryAllDto>>
            {
                Data = categories.Item1.Select(x => new CategoryAllDto
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    UpdatedAt = x.UpdatedAt,
                    CreatedAt = x.CreatedAt,
                    Slug = x.Slug
                    
                }),
                Message = "Categories found",
                Status = true,
                Total = categories.Item2,
            };
            if (paging != null)
            {
                var (page, pageSize, TotalPage) = Helpers.Paging.GetPaging(paging.Page, paging.PageSize,total);
                result.Page = page;
                result.PageSize = pageSize;
                result.TotalPage = TotalPage;
            }
            return result;
        }

        public async Task<ApiResponse<CategoryAllDto>> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return new ApiResponse<CategoryAllDto>
                {
                    Data = null,
                    Message = "Category not found",
                    Status = false
                };
            }
            return new ApiResponse<CategoryAllDto>
            {
                Data = new CategoryAllDto
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    Slug = category.Slug

                },
                Message = "Category found",
                Status = true
            };
        }

        public async Task<ApiResponse<CategoryAllDto>> GetCategoryBySlugAsync(string slug)
        {
            var category = await _categoryRepository.GetCategoryBySlugAsync(slug);
            if (category == null)
            {
                return new ApiResponse<CategoryAllDto>
                {
                    Data = null,
                    Message = "Category not found",
                    Status = false
                };
            }
            return new ApiResponse<CategoryAllDto>
            {
                Data = new CategoryAllDto
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    Slug = category.Slug
                },
                Message = "Category found",
                Status = true
            };
        }

        public async Task<ApiResponse<int>> UpdateCategoryAsync(int id, CategoryDto category)
        {
            var categoryToUpdate = new Category
            {
                Name = category.Name,
                Description = category.Description,
                UpdatedAt = DateTime.UtcNow,
                Slug = StringHelper.GenerateSlug(category.Name)
            };
            try
            {
                await _categoryRepository.UpdateCategoryAsync(id, categoryToUpdate);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = "Category updated",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = ex.Message,
                    Status = false
                };
            }
        }
    }
}
