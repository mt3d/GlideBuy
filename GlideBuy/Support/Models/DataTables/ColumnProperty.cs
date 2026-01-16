namespace GlideBuy.Support.Models.DataTables
{
	public class ColumnProperty
	{
		// data is the name of the property that should be read to populate this column.
		public ColumnProperty(string data)
		{
			Data = data;
			Visible = true;
		}

		// https://datatables.net/reference/option/columns.data
		public string Data { get; set; }

		// https://datatables.net/reference/option/columns.title
		public string Title { get; set; }

		// https://datatables.net/reference/option/columns.width
		public int Width { get; set; }

		// https://datatables.net/reference/option/columns.visible
		public bool Visible { get; set; }

		// https://datatables.net/reference/option/columns.searchable
		public bool Searchable { get; set; }
	}
}
