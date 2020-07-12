using System.Collections.Generic;
using System.Threading.Tasks;
using day2efcoredemo.DataContext;
using day2efcoredemo.Infra;
using day2efcoredemo.Models;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace day2efcoredemo.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        public UserService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<IList<User>> GetAll()
        {
            return _db.Users.ToList();
        }

        public async Task<LoginResponse> Login(LoginRequest req)
        {
            var selectedUser = _db.Users.SingleOrDefault(u => u.UserName == req.UserName && u.Password == req.Password);
            if(selectedUser != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["jwt-secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, selectedUser.Id.ToString()),
                        new Claim(ClaimTypes.Role, "admin")
                    }),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenStr = tokenHandler.WriteToken(token);

                return new LoginResponse(selectedUser, tokenStr);
            }

            return null;
        }
    }
}