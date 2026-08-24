using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public required string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public string? OrderNote { get; set; }
        public string? PaymentId { get; set; }
        public string? PaymentToken { get; set; }
        public string? ConversionId { get; set; }
        public EnumOrderState OrderState { get; set; }
        public EnumPaymentTypes PaymentTypes { get; set; }
        public List<OrderItem> OrderItems { get; set; }= new List<OrderItem>();

    }
    public enum EnumOrderState { waiting=0,unpaid=1,completed=2}
    public enum EnumPaymentTypes { CreditCard=0,Eft=1}
}
