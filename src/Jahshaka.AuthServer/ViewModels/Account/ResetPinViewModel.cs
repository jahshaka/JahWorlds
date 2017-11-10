using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Jahshaka.AuthServer.ViewModels.Account
{
    public class ResetPinViewModel : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(4, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Pin { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm pin")]
        [Compare("Pin", ErrorMessage = "The pin and confirmation pin do not match.")]
        public string ConfirmPin { get; set; }

        public string Code { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if(!Regex.IsMatch(Pin, @"^\d+$")){
                yield return new ValidationResult("Pin code must be numeric.", new[] { nameof(Pin) });
            }

        }
    }
}
