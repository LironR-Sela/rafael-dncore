using System.ComponentModel.DataAnnotations;

namespace day2efcoredemo.Models
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string username { get; set; }
        public string Token { get; set; }

        public LoginResponse(User user, string token)
        {
            Id = user.Id;
            FullName = $"{user.FirstName} {user.LastName}";
            username = user.UserName;
            Token = token;
        }
    }
}