using ECommerceAPI.Model;
using EcommerceNEWAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceNEWAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task<bool> UpdateProductAsync(int id, ProductDto productDto);
        Task<IEnumerable<ProductDto>> GetProductsWithCategoryAsync();  
        Task<IEnumerable<ProductDto>> GetProductsWithoutCategoryAsync();  
        Task<bool> DeleteProductAsync(int id);
    }
}
