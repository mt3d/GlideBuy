namespace GlideBuy.Services.Installation.SampleData
{
    public class SampleProducts
    {
        public List<SampleProduct> Products { get; set; } = new();

        public class SampleProduct
        {
            public string Name { get; set; } = string.Empty;

            public string ShortDescription { get; set; } = string.Empty;

            public decimal Price { get; set; }

            public bool Published { get; set; }

            public List<string> ProductPictures { get; set; } = new();
        }
    }
}
