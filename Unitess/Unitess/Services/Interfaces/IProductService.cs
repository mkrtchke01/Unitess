using Unitess.Models;
using Unitess.Requests;
using Unitess.Responses;

namespace Unitess.Services.Interfaces
{
    public interface IProductService
    {
        Task<GetProductResponse> GetAsync(int productId);
        Task<List<GetProductResponse>> GetAllAsync(GetAllProducts getAllProducts);
        Task CreateAsync(CreateProductRequest createProductRequest);
        Task UpdateAsync(UpdateProductRequest updateProductRequest, int productId);
        Task DeleteAsync(int productId);
    }
}
