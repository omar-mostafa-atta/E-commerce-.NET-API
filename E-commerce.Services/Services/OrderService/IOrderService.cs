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
		Task <IEnumerable<Order>> GetAllAsync();
		Task<Order> GetByIdAsync(int id);
		Task AddAsync(Order order);
		Task UpdateAsync(Order order);
		Task DeleteAsync(int id);

	}
}
