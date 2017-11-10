using System.ComponentModel.DataAnnotations;

namespace Jahshaka.AuthServer.ViewModels.Account
{
    public class ForgotPinViewModel {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
