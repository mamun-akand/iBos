using System.ComponentModel.DataAnnotations;

namespace WebApiTask.Models
{
    public class ColdDrinks
    {
        [Key]
        public int intColdDrinksId { get; set; }
        [Required]
        public string? strColdDrinksName { get; set; }
        public decimal numQuantity { get; set; }
        public decimal numUnitPrice { get; set; }
    }
}
