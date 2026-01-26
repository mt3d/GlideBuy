using GlideBuy.Core.Domain.Media;

namespace GlideBuy.Services.Media
{
	public interface IPictureService
	{
		Task<IList<Picture>> GetPicturesByProductAsync(int productId, int recordsToReturn = 0);
	}
}
