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
    public partial class InstallationService : IInstallationService
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
            // TODO: Remove in the future.
            if (_dbContext.Products.Any())
                return;


            await InstallCustomersAndUsersAsync();



            // TODO: Check if sample data needs to be installed

            var sampleData = JsonSerializer.Deserialize<SampleData.SampleData>(await _fileProvider.ReadAllTextAsync(_fileProvider.MapPath(InstallationDefaults.SampleDataPath), Encoding.UTF8));

            if (sampleData is null)
                return;

            await InstallCategoriesAsync(sampleData.Categories);
            await InstallProductsAsync(sampleData.Products);
        }
    }
}
