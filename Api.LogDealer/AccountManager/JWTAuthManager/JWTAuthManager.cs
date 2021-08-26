using Dapper;
using Api.LogDealer.AccountManager.Model;
using Api.LogDealer.Models;
using Api.LogDealer.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using System.Linq;

namespace Api.LogDealer.AccountManager.JWTAuthManager
{
    public class JWTAuthManager : BaseService, IJWTAuthManager
    {
        private readonly IConfiguration _configuration;

        public JWTAuthManager(IConfiguration configuration)
            :base(configuration)
        {
            _configuration = configuration;
        }
        private ResultModel<string> GenerateJWT<T>(User user)
        {

            //var currentUser = HttpContext.User;
            //DateTime TokenDate = new DateTime();

            //if (currentUser.HasClaim(c => c.Type == "Date"))
            //{
            //    TokenDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Date").Value);

            //}
            ResultModel<string> result  = new ResultModel<string>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtAuth:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claim is used to add identity to JWT token
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("roles", user.Role),
                new Claim("Date", DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_configuration["JwtAuth:Issuer"],
              _configuration["JwtAuth:Issuer"],
              claims,    //null original value
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            result.RetData = new JwtSecurityTokenHandler().WriteToken(token); //return access token
            result.RetValue = 200;
            //result.message = "Token generated";
            return result;

        }
        public ResultModel<string> Login<T>(LoginModel login)
        {
            ResultModel<string> result = new ResultModel<string>();

            DynamicParameters dp_param = new DynamicParameters();
            dp_param.Add("email", login.email, DbType.String);
            dp_param.Add("password", login.password, DbType.String);
            dp_param.Add("retVal", DbType.String, direction: ParameterDirection.Output);

            ResultModel<T> user = DBExecute<T>("sp_loginUser", dp_param);
            if (user.RetValue == 200)
            {
                result = GenerateJWT<T>((user.RetData as User));
                return result;
            }
            result.RetMessageError = "Utente non esistente o email e password sbagliate!";
            return result;

        }
        public ResultModel<T> RegisterUser<T>(User user)
        {
            ResultModel<T> result = new ResultModel<T>();

            DynamicParameters dp_param = new DynamicParameters();
            dp_param.Add("email", user.Email, DbType.String);
            dp_param.Add("username", user.Username, DbType.String);
            dp_param.Add("password", user.Password, DbType.String);
            dp_param.Add("role", user.Role, DbType.String);
            dp_param.Add("retVal", DbType.String, direction: ParameterDirection.Output);

            result = DBExecute<T>("sp_registerUser", dp_param);
            if (result.RetValue == 200)
            {
                return result;
            }
            result.RetMessageError = $"Registrazione non avvenuta o si riscontra un errore!";
            return result;
        }
        public ResultModel<T> DeleteUser<T>(string id)
        {
            ResultModel<T> result = new ResultModel<T>();

            DynamicParameters dp_param = new DynamicParameters();
            dp_param.Add("userid", id, DbType.String);
            dp_param.Add("retVal", DbType.String, direction: ParameterDirection.Output);
            result = DBExecute<T>("sp_deleteUser", dp_param);
            if (result.RetValue == 200)
            {
                return result;
            }
            result.RetMessageError = $"Eliminazione non avvenuta o si riscontra un errore!";
            return result;

        }
        public ResultModel<List<T>> GetUsers<T>()
        {
            string sql = "Select userid, username, email,[role], reg_date FROM dbo.T_ServiceUsers";
            return DBQuery<T>(sql);
        }
        public ResultModel<T> GetUser<T>(string email)
        {

            ResultModel<T> result = new ResultModel<T>();

            DynamicParameters dp_param = new DynamicParameters();
            //dp_param.Add("username", user.Username, DbType.String);
            dp_param.Add("email", email, DbType.String);
            dp_param.Add("retVal", DbType.String, direction: ParameterDirection.Output);
            result = DBExecute<T>("sp_getUser", dp_param);
            if (result.RetValue == 200)
            {
                return result;
            }
            result.RetMessageError = $"Utente non esistente o si riscontra un errore!";
            return result;


            //string sql = $"SELECT " +
            //    $"userid, username, email, phone, [role], reg_date " +
            //    $"FROM dbo.T_ServiceUsers " +
            //    $"WHERE username = '{user.Username}' " +
            //    $"AND email = '{user.Email}'";
            //return DBQuerySingleOfDefault<T>(sql);
        }
    }
}
