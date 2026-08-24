using System;

namespace ETICARET.WebUI.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateOn { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public double Rating { get; set; } = 5;
    }
}
