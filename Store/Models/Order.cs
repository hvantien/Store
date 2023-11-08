namespace Store.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingPostalCode { get; set; }
        public List<OrderItem> Items { get; set; }
        //public decimal TotalAmount
        //{
        //    get
        //    {
        //        return Items.Sum(item => item.Price * item.Quantity);
        //    }
        //}
    }
}
