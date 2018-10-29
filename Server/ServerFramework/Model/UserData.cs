namespace ServerFramework.Model
{
    class UserData
    {
        public UserData() { }
        public UserData( string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
