using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp2.API.Controllers
{
    public class CustomBaseController: ControllerBase
    {
        protected bool CheckRequestAuthorization(int userId)
        {
            var result = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId != int.Parse(result))
                return false;
            else
                return true;
        }
    }
}