using System.ComponentModel.DataAnnotations;

namespace tictactoeweb.Models.AuthorizationModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "incorrect username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "incorrect password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "incorrent confirm password")]
        [Compare("Password", ErrorMessage = "passwords are different")]
        public string PasswordConfirm { get; set; }
    }
}
