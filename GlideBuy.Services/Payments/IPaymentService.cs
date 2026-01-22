using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlideBuy.Services.Payments
{
	public interface IPaymentService
	{
		Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext orderPaymentContext);
	}
}
