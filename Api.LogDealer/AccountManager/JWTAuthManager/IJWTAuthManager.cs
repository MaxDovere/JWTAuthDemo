using Api.LogDealer.AccountManager.Model;
using Api.LogDealer.Models;
using System.Collections.Generic;

namespace Api.LogDealer.AccountManager.JWTAuthManager
{
    public interface IJWTAuthManager
    {
       //ResultModel<string> GenerateJWT(User user);
        ResultModel<List<T>> GetUsers<T>();
        ResultModel<T> GetUser<T>(string email);
        ResultModel<string> Login<T>(LoginModel login);
        ResultModel<T> RegisterUser<T>(User user);
        ResultModel<T> DeleteUser<T>(string id);

    }
}
