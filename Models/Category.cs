using System.ComponentModel.DataAnnotations;

namespace Budget_Management.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "No puede ser mayor a {1} caracteres.")]
        public string Name { get; set; }
        [Display(Name = "Tipo de operación")]
        public OperationType operationTypeId { get; set; }
        public int userId { get; set; }
    }
}
