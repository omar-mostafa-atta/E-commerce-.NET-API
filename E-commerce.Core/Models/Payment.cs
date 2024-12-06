using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Models
{
	public class Payment : BaseEntity
	{
	 

		public string NameOnCard { get; set; }
		public int CardNumber { get; set; }
		public int CVV {  get; set; }
		public  string ExpireDate { get; set; }
		public int OrderId { get; set; }
		public Order Order { get; set; }
		public int  UserId { get; set; }
		public ApplicationUser User { get; set; }

	}
}
