using System.ComponentModel.DataAnnotations;

namespace tictactoeweb.Models.AuthorizationModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "incorrect username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "incorrect password")]
        public string Password { get; set; }
    }
}
