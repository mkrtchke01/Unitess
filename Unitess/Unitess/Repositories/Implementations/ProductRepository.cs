using Dapper;
using System.Linq;
using Unitess.Common;
using Unitess.Models;
using Unitess.Repositories.Interfaces;
using Unitess.Requests;
using Unitess.Responses;

namespace Unitess.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        public async Task<GetProductResponse> GetAsync(int productId)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "SELECT * FROM Products WHERE ProductId = @ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("ProductId", productId);
            var product = sqlConnection.Query<GetProductResponse>(sqlQuery, parameters).FirstOrDefault();
            if (product == null)
            {
                throw new NotFoundException(nameof(product));
            }
            return product;
        }

        public async Task<List<GetProductResponse>> GetAllAsync(GetAllProducts getAllProducts)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "SELECT * FROM Products";
            var products = sqlConnection.Query<GetProductResponse>(sqlQuery)
                .Skip(getAllProducts.Skip)
                .Take(getAllProducts.Take)
                .ToList();
            return products;
        }

        public async Task Create(CreateProductRequest createProductRequest)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "INSERT INTO Products (ProductName, UserId) VALUES(@ProductName, @UserId)";
            var product = new Product()
            {
                ProductName = createProductRequest.ProductName,
                UserId = createProductRequest.UserId
            };
            await sqlConnection.ExecuteAsync(sqlQuery, product);
        }

        public async Task Update(UpdateProductRequest updateProductRequest, int productId)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "UPDATE Users SET ProductName = @ProductName, UserId = @UserId WHERE ProductId = @ProductId";
            var product = new Product()
            {
                ProductId = productId,
                ProductName = updateProductRequest.ProductName,
                UserId = updateProductRequest.UserId
            };
            await sqlConnection.ExecuteAsync(sqlQuery, product);
        }

        public async Task Delete(int productId)
        {
            await using var sqlConnection = DbConnection.CreateConnection();
            var sqlQuery = "DELETE FROM Products WHERE ProductId = @ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("ProductId", productId);
            await sqlConnection.ExecuteAsync(sqlQuery, parameters);
        }
    }
}
