using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Filters
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = this.GetRouteRedirect("Manage", "LogIn");
            }
            else
            {
                if (user.IsInRole("admin") == false)
                {
                    context.Result = this.GetRouteRedirect("Manage", "ErrorAcceso");
                }
            }
        }
        private RedirectToRouteResult GetRouteRedirect(string controller, string action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(new { controller = controller, action = action });
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
