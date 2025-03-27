using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        [Required]
        public string Address { get; set; } = string.Empty; 

        [Required]
        public string Contact { get; set; } = string.Empty;

        [Required]
        public string PaymentMethod { get; set; } = "Cash"; 

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
