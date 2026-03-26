using GlideBuy.Core;
using GlideBuy.Core.Domain.Catalog;
using GlideBuy.Core.Domain.Media;
using GlideBuy.Core.Infrastructure;
using GlideBuy.Data;
using GlideBuy.Models;
using GlideBuy.Services.Installation.SampleData;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace GlideBuy.Services.Installation
{
    public class InstallationService : IInstallationService
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

            // TODO: Remove in the future.
            if (_dbContext.Products.Any())
                return;

            var sampleData = JsonSerializer.Deserialize<SampleData.SampleData>(await _fileProvider.ReadAllTextAsync(_fileProvider.MapPath(InstallationDefaults.SampleDataPath), Encoding.UTF8));

            if (sampleData is null)
                return;

            await InstallCategoriesAsync(sampleData.Categories);
            await InstallProductsAsync(sampleData.Products);
        }

        protected virtual async Task InstallCategoriesAsync(List<SampleCategory> jsonData)
        {
            async Task<Category> createCategory(SampleCategory sample, int? parentCategoryId = null)
            {
                var category = new Category
                {
                    Name = sample.Name,
                    DisplayOrder = sample.DisplayOrder,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    ParentCategoryId = parentCategoryId
                };

                return category;
            }

            var allCategories = new List<Category>();
            var categoriesToInsert = new List<Category>();

            async Task saveCategory(SampleCategory sampleCategory, int? parentCategoryId = null)
            {
                var category = await createCategory(sampleCategory, parentCategoryId);
                allCategories.Add(category);

                if (sampleCategory.SubCategories.Any())
                {
                    _dbContext.Add(category);
                    await _dbContext.SaveChangesAsync();

                    foreach (var subCategory in sampleCategory.SubCategories)
                        await saveCategory(subCategory, category.Id);
                }
                else
                {
                    categoriesToInsert.Add(category);
                }
            }

            foreach (var sampleCategory in jsonData)
                await saveCategory(sampleCategory);

            _dbContext.AddRange(categoriesToInsert);
            await _dbContext.SaveChangesAsync();

            // TODO: Handle search engine names
        }

        protected virtual async Task InstallProductsAsync(SampleProducts jsonData)
        {
            var allProducts = new List<Product>();

            var categoryId = (await _dbContext.Categories.FirstOrDefaultAsync()).Id;

            async Task insertProduct(SampleProducts.SampleProduct sample)
            {
                var product = new Product
                {
                    Name = sample.Name,
                    ShortDescription = sample.ShortDescription,
                    Price = sample.Price,
                    Published = sample.Published,
                    // TODO: Fix
                    CategoryId = categoryId,
                };

                allProducts.Add(product);

                // TODO: Insert the entity so that it gets an ID
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                if (sample.ProductPictures.Any())
                {
                    var productPictures = new List<ProductPicture>();

                    foreach (var pictureName in sample.ProductPictures)
                    {
                        productPictures.Add(new()
                        {
                            ProductId = product.Id,
                            PictureId = await InsertPictureAsync(pictureName, product.Name),
                            DisplayOrder = 1,
                        });
                    }

                    _dbContext.AddRange(productPictures);
                    await _dbContext.SaveChangesAsync();
                }

                // TODO: Update allProducts
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
            var contentType = MimeTypes.ImagePng;

            var picture = new Picture
            {
                MimeType = contentType,
                AltAttribute = null,
                TitleAttribute = null,
                IsNew = true,
            };

            _dbContext.Pictures.Add(picture);
            _dbContext.PictureBinaries.Add(new PictureBinary { Picture = picture, BinaryData = pictureBinary });
            await _dbContext.SaveChangesAsync();

            return picture.Id;
        }
    }
}
