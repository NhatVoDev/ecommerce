﻿using ecommerce.DTO;

namespace ecommerce.Services.Interface
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryAllDto>>> GetAllCategoriesAsync(PagingForBlogCategory? paging = null);
        Task<ApiResponse<CategoryAllDto>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<CategoryAllDto>> GetCategoryBySlugAsync(string slug);
        Task<ApiResponse<int>> AddCategoryAsync(CategoryDto category);
        Task<ApiResponse<int>> DeleteCategoryAsync(int id);
        Task<ApiResponse<int>> UpdateCategoryAsync(int id, CategoryDto category);
        // delete categories
        Task<ApiResponse<int>> DeleteCategoriesAsync(List<int> ids);
    }
}
