using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorMissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessage = "ErrorMissingStock")]
        [RegularExpression("^\\d+$", ErrorMessage = "Le champ Stock doit être un entier.")]

        public string Stock { get; set; }

        [Required(ErrorMessage = "Price")]
        [RegularExpression("^[0-9]*\\.?[0-9]+$", ErrorMessage = "Le champ prix doit être un nombre positif.")]
        public string Price { get; set; }
    }
}
