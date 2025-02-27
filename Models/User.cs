namespace login.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? UserPoints { get; set; }
        public string? ImageP {get; set; }
        public string? PhoneNum { get; set; }
        public int? Age { get; set; }
        public string? Location { get; set; }
        public string? School { get; set; } 
        public string? Country { get; set; }
        public string? Password { get; set; }
        public string? ConfairmPassword { get; set;}
        public string Role { get; set; }  // Can be "Admin" or "User"
    }
}
