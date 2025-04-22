using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget_Management.Models
{
    public class CreationAccountViewModel : Account
    {
        public IEnumerable<SelectListItem> AccountType { get; set; }
    }
}   
