using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        /// <summary>
        /// Many to many Çoka çok ilişki 
        /// </summary>
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    }
}
