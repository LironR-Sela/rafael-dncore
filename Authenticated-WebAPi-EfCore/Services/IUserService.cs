using System.Collections.Generic;
using System.Threading.Tasks;
using day2efcoredemo.Models;

namespace day2efcoredemo.Infra{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest req);
        Task<IList<User>> GetAll();
    }
}