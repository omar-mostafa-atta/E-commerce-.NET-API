using E_commerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.OrderService
{
	public interface IOrderService
	{
		IEnumerable<Order> GetAll();
		Order GetById(int id);
		void Add(Order order);
		void Update(Order order);
		void Delete(int id);

	}
}
