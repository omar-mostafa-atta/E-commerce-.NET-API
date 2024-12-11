using E_commerce.Core.Models;
using E_commerce.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.PaymentService
{
	public class PaymentService:IPaymentService 
	{
		private readonly E_commerceContext _context;

		public PaymentService(E_commerceContext context)
		{
			_context = context;
		}

		public void Add(Payment payment)
		{
			_context.Payment.Add(payment);
			_context.SaveChanges();
		}

		public Payment GetByOrderId(int orderId)
		{
			return _context.Payment.FirstOrDefault(p => p.OrderId == orderId);
		}

	}
}
