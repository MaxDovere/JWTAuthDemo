using Api.LogDealer.AccountManager.JWTAuthManager;
using Api.LogDealer.AccountManager.Model;
using Api.LogDealer.GeoManager;
using Api.LogDealer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Api.LogDealer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class GeoController : ControllerBase
    {
        private readonly IGeoLocation _service;
        private readonly User currentUser = new User();
        private readonly IJWTAuthManager _authentication;
        public GeoController(IJWTAuthManager authentication, IGeoLocation service)
        {
            _service = service;
            _authentication = authentication;

        }
        [HttpGet("Coordinate")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetCoordinate(string servername)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Parameter is missing");
            }
            var resultUser = GetUserInfo<User>();
            if (resultUser.RetValue == 200)
            {
                User user = resultUser.RetData;

                var resultGeo = _service.SingleLatitudineLongitudine<LocationModel>(servername);
                if (resultGeo.RetValue == 200)
                {
                    return Ok(resultGeo);
                }
                return BadRequest(resultGeo);
            }
            return BadRequest("User Unauthorized or no valid!");
        }
        [HttpGet("Latitudine")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetLatitudine(string servername)
        {
            var resultUser = GetUserInfo<User>();
            if (resultUser.RetValue == 200)
            {
                User user = (User)resultUser.RetData;

                var result = _service.SingleLatitudine<LocationModel>(servername);
                if (result.RetValue == 200)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            return BadRequest("User unauthorized or no valid!");
        }
        [HttpGet("Longitudine")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetLongitudine(string servername)
        {
            var resultUser = GetUserInfo<User>();
            if (resultUser.RetValue == 200)
            {
                User user = (User)resultUser.RetData;

                var result = _service.SingleLongitudine<LocationModel>(servername);
                if (result.RetValue == 200)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            return BadRequest("User unauthorized or no valid!");
        }
        [HttpGet("CoordinateList")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<LocationModel>> GetCoordinateList(string dealer)
        {
            var resultUser = GetUserInfo<User>();
            if (resultUser.RetValue == 200)
            {
                User user = (User)resultUser.RetData;

                var result = _service.LatitudineLongitudine<LocationModel>(dealer);
                if (result.RetValue == 200)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            return BadRequest("User unauthorized or no valid!");
        }
        [HttpGet("InfoServerName")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetInfoObjectServerName(string servername)
        {
            var resultUser = GetUserInfo<User>();
            if (resultUser.RetValue == 200)
            {
                User user = (User)resultUser.RetData;

                var result = _service.SingleInfoObjectServerName<LocationModel>(servername);
                if (result.RetValue == 200)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            return BadRequest("User unauthorized or no valid!");
        }
        [HttpGet("Dealers")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<LocationModel>> GetInfoDealers()
        {
            var resultUser = GetUserInfo<User>();
            if (resultUser.RetValue == 200)
            {
                User user = (User)resultUser.RetData;

                var result = _service.InfoDealers<LocationModel>();
                if (result.RetValue == 200)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            return BadRequest("User unauthorized or no valid!");
        }
        private ResultModel<User> GetUserInfo<T>()
        {
            ResultModel<User> result = new ResultModel<User>();

            var currentUser = HttpContext.User;
            if (currentUser == null)
                return new ResultModel<User>()
                {
                    RetValue = 500,
                    RetData = null,
                    RetMessageError = "Utente non valido o non authorizzato!"
                };

            if (currentUser.HasClaim(c => c.Type.Contains("nameidentifier")))
            {

                string email = currentUser.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress")).Value;
                result = _authentication.GetUser<User>(email);
                if (result.RetValue == 200)
                {
                    return result;
                }
            }
            result.RetValue = 500;
            result.RetMessageError = "Utente non valido o non authorizzato!";
            return result;
        }
    }
}
