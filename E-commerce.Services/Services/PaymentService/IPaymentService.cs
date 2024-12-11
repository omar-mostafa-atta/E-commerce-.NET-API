using E_commerce.Core.Models;
using E_commerce.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.PaymentService
{
	public interface IPaymentService
	{
		void Add(Payment payment);
		Payment GetByOrderId(int id);
	}
}
