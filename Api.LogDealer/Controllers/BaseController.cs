using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Api.LogDealer.Controllers
{
    public abstract class BaseController: ControllerBase
    {
        protected string GetUserEmail()
        {
            return this.User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
        }
        protected string GetUserUserName()
        {
            return this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
        }
        protected ClaimsPrincipal GetCurrentUser 
        {
            get => HttpContext.User;
        }
    }
}
