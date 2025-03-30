using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceNEWAPI.DTOs;

namespace EcommerceNEWAPI.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDTO);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDTO);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
