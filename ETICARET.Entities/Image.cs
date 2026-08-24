using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ETICARET.Entities
{
    [Table("Image")]
    public class Image
    {
        public int Id { get; set; }
        /// <summary>
        /// Db tarafında Allow Null a izin vermek için ? işareti kullanılır.
        /// Product? ise null olabilir demektir.
        /// </summary>
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
