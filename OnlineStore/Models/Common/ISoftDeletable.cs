namespace OnlineStore.Models.Common
{
	/// <summary>
	/// Represents an entity that can be soft deleted.
	/// </summary>
	public interface ISoftDeletable
	{
		/// <summary>
		/// Gets or sets a value indicating weather the entity has been soft deleted or not.
		/// </summary>
		bool Deleted { get; set; }
	}
}
