namespace ECommerceApp.Models
{
    public class OrderRequestModel
    {
        public string Address { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "Cash";
        public List<OrderDetail> CartItems { get; set; } = new List<OrderDetail>();
    }


}
