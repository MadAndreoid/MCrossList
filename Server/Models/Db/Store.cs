using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCrossList.Server.Models.db
{
    [Table("Stores")]
    public partial class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}