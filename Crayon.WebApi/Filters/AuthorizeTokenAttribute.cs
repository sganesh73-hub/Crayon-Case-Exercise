using Crayon.WebApi.Models;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace Crayon.WebApi.Filters
{
    public class AuthorizeTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {

            var headers = filterContext.Request.Headers.Select(x => new { Key = x.Key, Value = x.Value.ToString() }).ToList();
            const string authTokenHeaderName = "AuthToken";

            if (headers.All(x => x.Key != authTokenHeaderName))
            {
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResponse { Code = 501, Message = "Missing Auth Token", ErrorCode = HttpStatusCode.BadRequest.ToString() });
                return;
            }

            var authTokenFromHeader = filterContext.Request.Headers.FirstOrDefault(x => x.Key == authTokenHeaderName).Value;
            var token = ((string[])authTokenFromHeader)[0];

            var authTokenFromConfig = ConfigurationManager.AppSettings["AuthToken"];
            if (token == authTokenFromConfig) return;
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiResponse { Code = 502, Message = "Invalid Auth Token", ErrorCode = HttpStatusCode.Unauthorized.ToString() });
        }
    }
}