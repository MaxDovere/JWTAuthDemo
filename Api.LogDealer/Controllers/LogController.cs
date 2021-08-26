using Api.LogDealer.AccountManager.JWTAuthManager;
using Api.LogDealer.AccountManager.Model;
using Api.LogDealer.LogManager;
using Api.LogDealer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.LogDealer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseController
    {
        private readonly ILogService _service;
        private readonly User currentUser = new User();
        private readonly IJWTAuthManager _authentication;
        public LogController(IJWTAuthManager authentication, ILogService service)
        {
            _service = service;
            _authentication = authentication;

        }
        
        // GET: api/<LogController>
        [HttpGet("Messages")]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetMessages(DateTime creationdate)
        {
            var result = _service.LogsList<LogMessageModel>(creationdate);
            if (result.RetValue == 200)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        // GET: api/<LogController>
        //[HttpGet("Messages")]
        //[AllowAnonymous]
        ////[Authorize(Roles = "Admin")]
        //public IActionResult GetMessages(string creationdate = "1900-01-01 00:00:00.000")
        //{
        //    var resultUser = GetUserInfo<User>();
        //    if (resultUser.RetValue == 200)
        //    {
        //        User user = (User)resultUser.RetData;

        //        var result = _service.LogsList<LogMessageModel>(creationdate);
        //        if (result.RetValue == 200)
        //        {
        //            return Ok(result);
        //        }
        //        return NotFound(result);
        //    }
        //    return BadRequest("User unauthorized or no valid!");
        //}
        // GET api/<LogController>/5
        [HttpGet("Message")]
        [Authorize(Roles = "User")]
        public IActionResult GetSingleMessage(int id)
        {
            var resultUser = GetUserInfo<User>();
            if (resultUser.RetValue == 200)
            {
                User user =  resultUser.RetData;

                var resultLog = _service.SingleLog<LogMessageModel>(id, user.UserId);
                if (resultLog.RetValue == 200)
                {
                    return Ok(resultLog);
                }
                return NotFound(resultLog);
            }
            return BadRequest("Utente non valido o non autorizzato!");
        }
        // GET api/<LogController>/5
        // POST api/<LogController>
        [HttpPost("AddMessage")]
        [Authorize] //(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddLog([FromBody] LogMessageModel log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parameter is missing");
            }
            var resultUser = GetUserInfo<User>();
            if(resultUser.RetValue == 200)
            {
                User user = resultUser.RetData;

                var resultLog = _service.AddLog<LogMessageModel>(log, user.UserId);
                if (resultLog.RetValue == 200)
                {
                    return Ok(resultLog);
                }
                return BadRequest(resultLog);
            }
            return BadRequest("Utente non valido o non autorizzato!");
        }
        // PUT api/<LogController>/5
        [HttpPut("UpdateMessage")]
        public IActionResult UpdateLog(int id, [FromBody] LogMessageModel log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parameter is missing");
            }
            var resultUser = GetUserInfo<User>();
            if(resultUser.RetValue == 200)
            {
                User user = resultUser.RetData;

                var result = _service.UpodateLog<LogMessageModel>(id, log, user.UserId);
                if (result.RetValue == 200)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Utente non valido o non autorizzato!");
        }
        // DELETE api/<LogController>/5
        [HttpDelete("DeleteMessage")]
        public IActionResult DeleteLog(int id)
        {

            var resultUser = GetUserInfo<User>();
            if(resultUser.RetValue == 200)
            {
                User user = resultUser.RetData;

                var resultLog = _service.DeleteLog<LogMessageModel>(id, user.UserId);
                if (resultLog.RetValue == 200)
                {
                    return Ok(resultLog);
                }
                return NotFound(resultLog);
            }
            return BadRequest("Utente non valido o non autorizzato!");
        }
        private ResultModel<T> GetUserInfo<T>()
        {
            ResultModel<T> result = new ResultModel<T>();

            var current = GetCurrentUser; //
            
            if (current == null)
                return new ResultModel<T>()
                {
                    RetValue = 500,
                    //RetData = null,
                    RetMessageError = "Utente non valido o non autorizzato!"
                };

            result = _authentication.GetUser<T>(GetUserEmail());
            if (result.RetValue == 200)
            {
                return result;
            }
            result.RetMessageError = "Utente non valido o non autorizzato!";
            //result.RetMessageError = $"Utente non valido o non authorizzato! {result.RetMessageError}";
            return result;
        }

    }
}
