﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.DTO
{
	public class ConfirmApplicationUserDTO
	{
		public string UserName { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public string OTP { get; set; }
	}
}
