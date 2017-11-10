using System.ComponentModel.DataAnnotations;

namespace Jahshaka.AuthServer.ViewModels.Account
{
    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
