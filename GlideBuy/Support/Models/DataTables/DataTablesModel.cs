namespace GlideBuy.Support.Models.DataTables
{
	// All these properties will passed to the constructor of DataTables().
	public class DataTablesModel
	{
		public object? Data { get; set; }

		public string Name { get; set; }

		// Corresponds to the 'paging' option in DataTables.
		public bool Paging { get; set; }

		// Coressponds to 'columns' in DataTables.
		// https://datatables.net/reference/option/columns
		public IList<ColumnProperty> ColumnCollection { get; set; }

		public DataUrl UrlRead { get; set; }
	}
}
