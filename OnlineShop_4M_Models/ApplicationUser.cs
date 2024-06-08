using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop_4M_Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
		[NotMapped]
		public string City { get; set; }
	}
}

