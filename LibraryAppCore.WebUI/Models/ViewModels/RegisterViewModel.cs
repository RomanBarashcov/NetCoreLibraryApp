using System.ComponentModel.DataAnnotations;

namespace LibraryAppCore.WebUI.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }
       
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Incorrect password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}