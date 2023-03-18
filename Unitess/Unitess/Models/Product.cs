namespace Unitess.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
