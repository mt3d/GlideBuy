using System.Text.Json.Serialization;

namespace GlideBuy.Support.Models
{
	public record BasePagedListModel<T> : BaseModel, IPagedModel<T> where T : BaseModel
	{
		[JsonPropertyName("Data")]
		public IEnumerable<T> Data { get; set; }

		[JsonPropertyName("draw")]
		public string Draw { get; set; }

		[JsonPropertyName("recordsFiltered")]
		public int RecordsFiltered { get; set; }

		[JsonPropertyName("recordsTotal")]
		public int RecordsTotal { get; set; }
	}
}
