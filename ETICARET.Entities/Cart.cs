using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ETICARET.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public List<CartItem> CartItems { get; set; }= new List<CartItem>();//one-to-many
    }
    public class CartItem 
    {
        public int Id { get; set; }
        public int ProductId { get; set; }//Foreign key
        public Product? Product { get; set; }//Navigator Property
        public Cart? Cart { get; set; }
       
        public int CartId { get; set; }//Foreign key
        public int Quantity { get; set; }
    }
}
