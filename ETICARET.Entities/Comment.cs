using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public required string UserId { get; set; }
        public DateTime CreateOn { get; set; }
        public double Rating { get; set; } = 5;

    }
}
