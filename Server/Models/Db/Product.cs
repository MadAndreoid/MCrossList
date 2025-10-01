using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCrossList.Server.Models.db
{
    [Table("Products")]
    public partial class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Dimension { get; set; }

        public long? Product_Store_ID { get; set; }

        public Store Product_Store { get; set; }

        public long? Product_Category_ID { get; set; }

        public Category Product_Category { get; set; }

        public long? Product_Brand_ID { get; set; }

        public Brand Product_Brand { get; set; }

        public long? Product_Size_ID { get; set; }

        public Size Product_Size { get; set; }

        public long? Product_Condition_ID { get; set; }

        public Condition Product_Condition { get; set; }

        public long? Product_Color_ID { get; set; }

        public Color Product_Color { get; set; }

        public long? Product_Material_ID { get; set; }

        public Material Product_Material { get; set; }
    }
}