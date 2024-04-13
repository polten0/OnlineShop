using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop_4M_Models
{
	public class Company
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }
    }
}

