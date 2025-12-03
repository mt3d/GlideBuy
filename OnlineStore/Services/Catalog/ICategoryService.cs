using GlideBuy.Models;

namespace GlideBuy.Services.Catalog
{
	public interface ICategoryService
	{
		Task<Category?> GetCategoryByIdAsync(int id);
	}
}
