using GlideBuy.Core.Domain.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Data
{
	public class SeedData
	{
		public static async Task EnsurePopulated(IApplicationBuilder app)
		{
		}

		private const string adminPassword = "Secret123$";
	}
}
