using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget_Management.Models
{
    public class TransactionCreationViewModel : Transaction
    {
        public IEnumerable<SelectListItem> Account { get; set; }
        public IEnumerable<SelectListItem> Category { get; set; }
    }
}
