using E_commerce.Core.Models;
using E_commerce.Repository.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IGenericRepository<Order> _orderRepository;

		public OrderService(IGenericRepository<Order> orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<IEnumerable<Order>> GetAllAsync()
		{
			return await _orderRepository.GetAllAsync(); // Await async method
		}

		public async Task<Order> GetByIdAsync(int id)
		{
			return await _orderRepository.GetByIdAsync(id); // Await async method
		}

		public async Task AddAsync(Order order)
		{
			await _orderRepository.AddAsync(order); // Await async method
		}

		public async Task UpdateAsync(Order order)
		{
			await _orderRepository.UpdateAsync(order); // Await async method
		}

		public async Task DeleteAsync(int id)
		{
			await _orderRepository.DeleteAsync(id); // Await async method
		}
	}
}
