using GlideBuy.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlideBuy.Services.Orders
{
	public class PlaceOrderResult
	{
		public IList<string> Errors { get; set; } = new List<string>();

		public bool Success => !Errors.Any();

		public Order PlacedOrder { get; set; }

		public void AddError(string error)
		{
			Errors.Add(error);
		}
	}
}
