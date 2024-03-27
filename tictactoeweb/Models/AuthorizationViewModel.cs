namespace tictactoeweb.Models
{
    public class AuthorizationViewModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
    }
}
