using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Models
{
	public class ApplicationUser:IdentityUser
	{
        public string? Address { get; set; }

		public int PaymentId { get; set; }
		public List<Payment> Payments { get; set; }
		List<Order> Orders { get; set; }
    }
}
