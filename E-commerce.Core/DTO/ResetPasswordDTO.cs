﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.DTO
{
	public class ResetPasswordDTO
	{
		public string Email { get; set; }
		public string OTP { get; set; }
		public string NewPassword { get; set; }
	}
}
