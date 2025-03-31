using System.ComponentModel.DataAnnotations;
using Budget_Management.Validaciones;

namespace Budget_Management.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} debe estar lleno.")]
        [Display(Name = "Nombre")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La cantidad de caracteres debe ser mayor o igual a {2} o menor o igual a {1}")]
        [IsFirstLetterCapital]
        public string name { get; set; }
        [Display(Name = "Nombre de usuario")]
        public int userId { get; set; }
        public int order { get; set; }

        public int age{ get; set; }
        public string email { get; set; }
        public int creditCard { get; set; }
    }
}
