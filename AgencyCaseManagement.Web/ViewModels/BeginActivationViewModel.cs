using System.ComponentModel.DataAnnotations;

namespace AgencyCaseManagement.Web.ViewModels
{
    public class BeginActivationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
