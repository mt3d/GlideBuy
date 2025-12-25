namespace GlideBuy.Models
{
	/// <summary>
	/// The base class for all database entities. Necessary for implementing
	/// a generic repository for all types of entities.
	/// </summary>
	public abstract class BaseEntity
	{
		public int Id { get; set; }
	}
}
