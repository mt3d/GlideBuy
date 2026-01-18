using GlideBuy.Core.Domain.Common;
using GlideBuy.Models;

namespace GlideBuy.Services.Common
{
	public interface IGenericAttributeService
	{
		Task<IList<GenericAttribute>> GetAttributesForEntityAsync(int entityId, string keyGroup);

		Task DeleteAttributeAsync(GenericAttribute genericAttribute);

		Task InsertAttributeAsync(GenericAttribute genericAttribute);

		Task UpdateAttributeAsync(GenericAttribute genericAttribute);

		Task SaveAttributeAsync<TPropType>(BaseEntity entity, string key, TPropType? value);

		Task<TPropType?> GetAttributeAsync<TPropType>(BaseEntity entity, string key, TPropType? defaultValue = default);
	}
}
