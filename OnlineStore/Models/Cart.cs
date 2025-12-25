using GlideBuy.Core.Domain.Orders;

namespace GlideBuy.Models
{
	public class Cart
	{
		public List<ShoppingCartItem> Lines { get; set; } = new List<ShoppingCartItem>();

		public virtual void AddItem(Product product, int quantity)
		{
			ShoppingCartItem? line = Lines.Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();

			if (line == null)
			{
				Lines.Add(new ShoppingCartItem { Product = product, Quantity = quantity });
			} else
			{
				line.Quantity += quantity;
			}
		}

		public virtual void RemoveLine(Product product)
		{
			Lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
		}

		public decimal ComputeTotalValues() => Lines.Sum(e => e.Product.Price * e.Quantity);

		public virtual void Clear() => Lines.Clear();
	}
}
