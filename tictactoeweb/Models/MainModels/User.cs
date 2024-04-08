using System.ComponentModel.DataAnnotations;

namespace tictactoeweb.Models.MainModels
{
    public class User
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "incorrect username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "incorrect password")]
        public int Games { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public string? Password { get; set; }
        public Role? Role { get; set; }
        public int RoleId { get; set; }
    }
}
