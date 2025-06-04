using System.ComponentModel.DataAnnotations;

namespace Budget_Management.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Fecha de la transacción")]
        [DataType(DataType.Date)]
        public DateTime DateTransaction { get; set; } = DateTime.Today;
        public decimal Amount { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }
        [StringLength(maximumLength: 1000, ErrorMessage = "La nota no puede exceder los {1} caracteres")]
        public string Note { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Cuenta")]
        public int AccountId { get; set; }
    }
}
