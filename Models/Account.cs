using System.ComponentModel.DataAnnotations;
using Budget_Management.Validaciones;

namespace Budget_Management.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [IsFirstLetterCapital]
        public string Name { get; set; }
        [Display(Name = "Tipo de cuenta")]
        public int AccountTypeId { get; set; }  
        public decimal Balance { get; set; }
        [StringLength(maximumLength:1000)]
        public string Description { get; set; }

        public string AccountType { get; set; }

    }
}
