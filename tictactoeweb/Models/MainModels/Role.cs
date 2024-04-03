namespace tictactoeweb.Models.MainModels
{
    public class Role
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public List<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}
