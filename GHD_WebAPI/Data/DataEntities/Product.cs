namespace GHD_WebAPI.Data.DataEntities
{

    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
