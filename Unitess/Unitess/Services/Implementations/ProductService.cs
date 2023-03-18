using Unitess.Common;
using Unitess.Models;
using Unitess.Repositories.Interfaces;
using Unitess.Requests;
using Unitess.Responses;
using Unitess.Services.Interfaces;

namespace Unitess.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<GetProductResponse> GetAsync(int productId)
        {
            var product = await _productRepository.GetAsync(productId);
            return product;
        }

        public async Task<List<GetProductResponse>> GetAllAsync(GetAllProducts getAllProducts)
        {
            var products = await _productRepository.GetAllAsync(getAllProducts);
            return products;
        }

        public async Task CreateAsync(CreateProductRequest createProductRequest)
        {
            if (!await _userRepository.HasUserAsync(createProductRequest.UserId))
            {
                throw new NotFoundException(nameof(User));
            }
            await _productRepository.Create(createProductRequest);
        }

        public async Task UpdateAsync(UpdateProductRequest updateProductRequest, int productId)
        {
            if (!await _userRepository.HasUserAsync(updateProductRequest.UserId))
            {
                throw new NotFoundException(nameof(User));
            }
            await _productRepository.Update(updateProductRequest, productId);
        }

        public async Task DeleteAsync(int productId)
        {
            await _productRepository.Delete(productId);
        }
    }
}
