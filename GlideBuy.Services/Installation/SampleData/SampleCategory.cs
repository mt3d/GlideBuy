namespace GlideBuy.Services.Installation.SampleData
{
    public class SampleCategory
    {
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public string Icon { get; set; }

        public List<SampleCategory> SubCategories { get; set; } = new();
    }
}
