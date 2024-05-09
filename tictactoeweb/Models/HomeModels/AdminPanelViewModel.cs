using tictactoeweb.Models.AuthorizationModels;
using tictactoeweb.Models.MainModels;

namespace tictactoeweb.Models.HomeModels
{
    public class AdminPanelViewModel
    {
        public List<User> UserList { get; set; }
        public RegisterModel AdminCreate { get; set; }
    }
}
