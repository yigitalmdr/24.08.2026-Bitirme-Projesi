using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETICARET.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; } = 100;
        public List<Image> Images { get; set; } = new List<Image>();
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory> ();
        public List<Comment> Comments { get; set; } = new List<Comment> ();
    }
}
