using Unitess.Models;
using Unitess.Requests;
using Unitess.Responses;

namespace Unitess.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<GetProductResponse> GetAsync(int productId);
        Task<List<GetProductResponse>> GetAllAsync(GetAllProducts getAllProducts);
        Task Create(CreateProductRequest createProductRequest);
        Task Update(UpdateProductRequest updateProductRequest, int productId);
        Task Delete(int productId);
    }
}
