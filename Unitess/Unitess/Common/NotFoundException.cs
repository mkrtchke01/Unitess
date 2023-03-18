namespace Unitess.Common
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name) : base($"Entity {name.ToUpper()} is not found")
        {
        }
    }
}
