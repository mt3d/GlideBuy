namespace GlideBuy.Models.Catalog
{
    public record ProductPriceModel
    {
        public string OldPrice { get; set; }
        public decimal? OldPriceValue { get; set; }

        public string Price { get; set; }
        public decimal? PriceValue { get; set; }
    }
}
