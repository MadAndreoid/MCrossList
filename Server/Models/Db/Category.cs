using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCrossList.Server.Models.db
{
    [Table("Categories")]
    public partial class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Name { get; set; }

        public long? Category_Father_ID { get; set; }

        public Category Category_Father { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Category> InverseCategory_Father { get; set; }
    }
}