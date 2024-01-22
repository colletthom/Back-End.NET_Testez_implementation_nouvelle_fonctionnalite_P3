using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }

        [Required(ErrorMessage = "ErrorMissingName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price")]
        [RegularExpression("^[0-9]*\\.?[0-9]+$", ErrorMessage = "Le champ prix doit être un nombre positif.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "ErrorMissingStock")]
        [RegularExpression("^\\d+$", ErrorMessage = "Le champ Stock doit être un entier.")]
        public int Quantity { get; set; }

        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
