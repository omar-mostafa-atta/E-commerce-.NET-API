using E_commerce.Core.Models;
using E_commerce.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.OrderService
{
	public class OrderService:IOrderService
	{
		private readonly IGenericRepository<Order> _OrderRepository;
		public OrderService(IGenericRepository<Order> OrderRepository)
		{
			_OrderRepository = OrderRepository;
		}

		public IEnumerable<Order> GetAll()
		{
			return _OrderRepository.GetAll();
		}

		public Order GetById(int id)
		{
			return _OrderRepository.GetById(id);
		}

		public void Add(Order order)
		{
			
			_OrderRepository.Add(order);
		}

		public void Update(Order order)
		{
			_OrderRepository.Update(order);
		}

		public void Delete(int id)
		{
			_OrderRepository.Delete(id);
		}

	}
}
