using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop_4M_Models
{
	public class InquiryHeader
	{
		[Key]
		public int Id { get; set; }

		public string ApplicationUserId { get; set; }

		[ForeignKey("ApplicationUserId")]
		public ApplicationUser ApplicationUser { get; set; }

		// дата регистрации запроса
		public DateTime InquiryDate { get; set; }

		// данные если их клиент решил поменять
		[Required]
		public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

