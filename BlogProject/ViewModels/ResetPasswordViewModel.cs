using System.ComponentModel.DataAnnotations;

namespace BlogProject.Web.ViewModels
{
    public class ResetPasswordViewModel
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fill in the 'Password' field.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = null!;


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fill in the 'Confirm Password' field.")]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
