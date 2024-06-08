using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop_4M_Models
{
    public class OrderHeader
    {
        [Key] 
        public int Id { get; set; }
        public string CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public ApplicationUser CreatedBy { get; set; }
        public DateTime OrderDate { get; set; } //
        [Required]
        public double OrderTotal { get; set; } // 
        public string OrderStatus { get; set; }
        public string PhoneNumber { get; set; } //
        public string Email { get; set; } // 
        [Required]
        public string FullName { get; set; } //
        public string City { get; set; } //

        public DateTime PaymentDate { get; set; } //
        public string TransactionId { get; set; }

    }
}
