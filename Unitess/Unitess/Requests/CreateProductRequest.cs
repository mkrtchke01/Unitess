using Unitess.Models;

namespace Unitess.Requests
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; }
        public int UserId { get; set; }
    }
}
