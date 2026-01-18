using GlideBuy.Core;
using GlideBuy.Core.Domain.Common;
using GlideBuy.Data;
using GlideBuy.Models;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Services.Common
{
	public class GenericAttributeService : IGenericAttributeService
	{
		private readonly IDataRepository<GenericAttribute> _genericAttributeRepository;

		public GenericAttributeService(IDataRepository<GenericAttribute> genericAttributeRepository)
		{
			_genericAttributeRepository = genericAttributeRepository;
		}

		public async Task<IList<GenericAttribute>> GetAttributesForEntityAsync(int entityId, string keyGroup)
		{
			var query = _genericAttributeRepository.Table.Where(ga => ga.EntityId == entityId && ga.KeyGroup == keyGroup);

			// TODO: Use short term cache manager
			var attributes = await query.ToListAsync();

			return attributes;
		}

		public async Task DeleteAttributeAsync(GenericAttribute genericAttribute)
		{
			await _genericAttributeRepository.DeleteAsync(genericAttribute);
		}

		// TODO: Remove?
		public async Task InsertAttributeAsync(GenericAttribute genericAttribute)
		{
			ArgumentNullException.ThrowIfNull(genericAttribute);

			genericAttribute.CreatedOrUpdatedAtUtc = DateTime.UtcNow;

			await _genericAttributeRepository.InsertAsync(genericAttribute);
		}

		public async Task UpdateAttributeAsync(GenericAttribute genericAttribute)
		{
			ArgumentNullException.ThrowIfNull(genericAttribute);

			genericAttribute.CreatedOrUpdatedAtUtc = DateTime.UtcNow;

			await _genericAttributeRepository.UpdateAsync(genericAttribute);
		}

		public async Task SaveAttributeAsync<TPropType>(BaseEntity entity, string key, TPropType value)
		{
			ArgumentNullException.ThrowIfNull(entity);
			ArgumentNullException.ThrowIfNull(key);

			var keyGroup = entity.GetType().Name;

			// Check if the attribute already exists
			var attrs = (await GetAttributesForEntityAsync(entity.Id, keyGroup));
			var attr = attrs.FirstOrDefault(ga => ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

			var valueStr = ConversionHelper.ConvertTo<string>(value);

			if (attr is not null)
			{
				if (string.IsNullOrEmpty(valueStr))
				{
					await DeleteAttributeAsync(attr);
				}
				else
				{
					attr.Value = valueStr;
					await UpdateAttributeAsync(attr);
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(valueStr))
				{
					return;
				}

				attr = new GenericAttribute
				{
					EntityId = entity.Id,
					KeyGroup = keyGroup,
					Key = key,
					Value = valueStr,
				};

				await InsertAttributeAsync(attr);
			}
		}
	}
}
