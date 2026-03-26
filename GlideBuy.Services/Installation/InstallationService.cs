using GlideBuy.Core;
using GlideBuy.Core.Domain.Catalog;
using GlideBuy.Core.Domain.Media;
using GlideBuy.Core.Infrastructure;
using GlideBuy.Data;
using GlideBuy.Models;
using GlideBuy.Services.Installation.SampleData;
using System.Text;
using System.Text.Json;

namespace GlideBuy.Services.Installation
{
    public class InstallationService
    {
        protected readonly IGlideBuyFileProvider _fileProvider;
        protected readonly StoreDbContext _dbContext;

        public InstallationService(
            IGlideBuyFileProvider fileProvider,
            StoreDbContext storeDbContext)
        {
            _fileProvider = fileProvider;
            _dbContext = storeDbContext;
        }

        // TODO: Use installation settings
        public virtual async Task InstallAsync()
        {
            // TODO: Check if sample data needs to be installed

            // TODO: Move path to a defaults class
            var sampleData = JsonSerializer.Deserialize<SampleData.SampleData>(await _fileProvider.ReadAllTextAsync(_fileProvider.MapPath(InstallationDefaults.SampleDataPath), Encoding.UTF8));

            if (sampleData is null)
                return;

            await InstallProductsAsync(sampleData.Products);
        }

        protected virtual async Task InstallProductsAsync(SampleProducts jsonData)
        {
            var allProducts = new List<Product>();

            async Task insertProduct(SampleProducts.SampleProduct sample)
            {
                var product = new Product
                {
                    Name = sample.Name,
                    ShortDescription = sample.ShortDescription,
                    Price = sample.Price,
                    Published = sample.Published,
                };

                allProducts.Add(product);

                // TODO: Insert the entity so that it gets an ID
                _dbContext.Products.Add(product);

                if (sample.ProductPictures.Any())
                {
                    var productPictures = new List<ProductPicture>();

                    foreach (var pictureName in sample.ProductPictures)
                    {
                        productPictures.Add(new()
                        {
                            ProductId = product.Id,
                            // TODO: Insert the picture
                            PictureId = await InsertPictureAsync(pictureName, product.Name),
                            DisplayOrder = 1,
                        });
                    }
                }
            }

            foreach (var sample in jsonData.Products)
                await insertProduct(sample);
        }

        protected virtual async Task<int> InsertPictureAsync(string fileName, string name)
        {
            var sampleImagesPath = _fileProvider.GetAbsolutePath(InstallationDefaults.SampleImagesPath);

            var pictureBinary = await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, fileName));
            // TODO: Handle seo name

            // TODO: Use file extensions content type provider
            var contentType = MimeTypes.ImageJpeg;

            var picture = new Picture
            {
                MimeType = contentType,
                AltAttribute = null,
                TitleAttribute = null,
                IsNew = true,
            };

            _dbContext.Pictures.Add(picture);
            // TODO: Add image binary data
            await _dbContext.SaveChangesAsync();

            return picture.Id;
        }
    }
}
