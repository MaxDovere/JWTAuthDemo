using Api.LogDealer.AccountManager.JWTAuthManager;
using Api.LogDealer.AccountManager.Model;
using Api.LogDealer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.LogDealer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IJWTAuthManager _authentication;
        public AccountController(IJWTAuthManager authentication)
        {
            _authentication = authentication;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult login([FromBody]LoginModel user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Parameter is missing");
            }

            var result = _authentication.Login<User>(user);
            if (result.RetValue == 200)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [HttpGet("GetCurrentUser")]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {

            DateTime TokenDate = new DateTime();

            if (GetCurrentUser.HasClaim(c => c.Type == "Date"))
            {
                TokenDate = DateTime.Parse(GetCurrentUser.Claims.FirstOrDefault(c => c.Type == "Date").Value);

            }

            return Ok("Custom Claims(date): " + TokenDate);

        }
        [HttpGet("GetLoginUser")]
        [Authorize]
        public IActionResult GetUser(string email)
        {
            var result = _authentication.GetUser<User>(email);
            return Ok(result);

        }
        [HttpGet("UserList")]
        [Authorize(Roles = "Admin")]
        public IActionResult getAllUsers()
        {
            var result = _authentication.GetUsers<User>();
            return Ok(result);
        }

        [HttpPost("Register")]
        [Authorize(Roles = "Admin")]
        public IActionResult Register([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parameter is missing");
            }

            var result = _authentication.RegisterUser<User>(user);
            if (result.RetValue == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            if (id == string.Empty)
            {
                return BadRequest("Parameter is missing");
            }

            var result = _authentication.DeleteUser<User>(id);
            if (result.RetValue == 200)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
