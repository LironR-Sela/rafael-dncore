using System.ComponentModel.DataAnnotations;

namespace day2efcoredemo.Models
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}