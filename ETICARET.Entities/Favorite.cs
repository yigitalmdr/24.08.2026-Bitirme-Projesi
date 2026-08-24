using System;

namespace ETICARET.Entities
{
    public class Favorite
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
