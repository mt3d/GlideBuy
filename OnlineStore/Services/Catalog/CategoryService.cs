using GlideBuy.Data;
using GlideBuy.Models;

namespace GlideBuy.Services.Catalog
{
	public class CategoryService : ICategoryService
	{
		private readonly IDataRepository<Category> _dataRepository;

		public CategoryService(IDataRepository<Category> dataRepository)
		{
			_dataRepository = dataRepository;
		}

		public Task<Category?> GetCategoryByIdAsync(int id)
		{
			return _dataRepository.GetByIdAsync(id, cacheBuilder => default);
		}
	}
}
