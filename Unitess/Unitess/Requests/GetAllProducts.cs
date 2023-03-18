namespace Unitess.Requests
{
    public class GetAllProducts
    {
        public int Skip { get; set; }
        public int Take { get; set; } = Int32.MaxValue;
    }
}
