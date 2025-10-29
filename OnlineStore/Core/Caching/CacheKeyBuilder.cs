using GlideBuy.Models;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace GlideBuy.Core.Caching
{
	/// <summary>
	/// The default cache key implementation.
	/// 
	/// Provides a consistent way to generate cache keys for data that may depend
	/// on multiple parameters, like language, customer role, store, product category, etc.
	/// </summary>
	public abstract class CacheKeyBuilder : ICacheKeyBuilder
	{
		protected virtual object NormalizeKeyParameter(object parameter)
		{
			// If the object to be inserted using string.Format() is not a string,
			// its ToString method is called to convert it to one before inserting it in the result string.
			return parameter switch
			{
				// In normal cases, string.Format() inserts String.Empty if the argument is null.
				null => "null",
				// Would have returned List.ToString() for instance,
				// which is just the name of the class.
				IEnumerable<int> ids => GenerateIdsHash(ids),
				IEnumerable<BaseEntity> entities => GenerateIdsHash(entities.Select(entity => entity.Id)),
				// To avoid localized number format (depends on culture),
				// otherwise cache key might differ between servers (e.g. 3,5 vs 3.5).
				decimal param => param.ToString(CultureInfo.InvariantCulture),
				_ => parameter
			};
		}

		/// <summary>
		/// When caching results that depend on many IDs (like ProductIds), we don’t
		/// want very long keys like products.byids.1,2,3,4,...
		/// Instead, this method generates a SHA1 hash of the sorted IDs.
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		protected virtual string GenerateIdsHash(IEnumerable<int> ids)
		{
			var idList = ids.ToList();
			
			if (!idList.Any())
			{
				return string.Empty;
			}

			var idsString = string.Join(", ", idList.OrderBy(id => id));

			return HashHelper.CreateHash(Encoding.UTF8.GetBytes(idsString), "SHA1");
		}

		/// <summary>
		/// Clone the given cache key and fill in its parameters using the normalization
		/// logic.
		/// <example>
		/// <code>
		/// var cacheKey = new CacheKey("products.bycategory.{0}", categoryId);
		/// var prepared = _cacheKeyService.PrepareKeyForDefaultCache(cacheKey, categoryId);
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="cacheKeyParams"></param>
		/// <returns></returns>
		public CacheKey BuildKey(CacheKey key, params object[] cacheKeyParams)
		{
			// Internally, Create simply formats the key string, and fill it with
			// the supplied parameters. CreateCacheKeyParameters transforms
			// the params before handing them to string.Format().
			return key.Create(NormalizeKeyParameter, cacheKeyParams);
		}

		public CacheKey BuildKeyWithDefaultCacheTime(CacheKey key, params object[] cacheKeyParams)
		{
			throw new NotImplementedException();
		}
	}

	// TODO: Explain this class.
	public class HashHelper
	{
		/// <summary>
		/// Generate a hash string (e.g., SHA1, SHA256, MD5, etc.) from a given byte
		/// array using a named cryptographic algorithm.
		/// 
		/// If trimByteCount is specified, then only the first N bytes (defined by
		/// trimByteCount) are used to compute the hash.
		/// This can be useful for very large inputs where only a subset is needed
		/// for a consistent short hash (e.g., caching or quick identity check).
		/// </summary>
		/// <param name="data"></param>
		/// <param name="hashAlgorithm"></param>
		/// <param name="trimByteCount"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static string CreateHash(byte[] data, string hashAlgorithm, int trimByteCount = 0)
		{
			ArgumentException.ThrowIfNullOrEmpty(hashAlgorithm);

			// Example inputs and outputs:
			// "SHA1" => SHA1CryptoServiceProvider
			// "MD5" => MD5CryptoServiceProvider
			// "SHA256" => SHA256Managed
			var algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithm) ?? throw new ArgumentException("Unrecognized hash name");

			if (trimByteCount > 0 && data.Length > trimByteCount)
			{
				var newData = new byte[trimByteCount];
				Array.Copy(data, newData, trimByteCount);

				// BitConverter.ToString() turns that the byte array a hex string like "A4-9F-11-BA...".
				return BitConverter.ToString(algorithm.ComputeHash(newData)).Replace("-", string.Empty);
			}

			return BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);
		}
	}
}
