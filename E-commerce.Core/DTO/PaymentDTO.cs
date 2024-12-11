using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.DTO
{
	public class PaymentDTO
	{
		public string NameOnCard { get; set; }
		public int CardNumber { get; set; }
		public int CVV { get; set; }
		public string ExpireDate { get; set; }
		public int OrderId { get; set; }
		public string UserId { get; set; }
	}
}
