
using System.ComponentModel.DataAnnotations;


namespace E_commerce.Core.DTO
{
	public class RoleDTO
	{
		[Display(Name ="Role Name")]
		public string RoleName { get; set; }
	}
}
