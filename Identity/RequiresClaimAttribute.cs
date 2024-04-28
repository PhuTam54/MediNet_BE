using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MediNet_BE.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresClaimAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _claimName;
        private readonly string _claimValue;

        public RequiresClaimAttribute(string claimName, string claimValue)
        {
            _claimName = claimName;
            _claimValue = claimValue;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (!filterContext.HttpContext.User.HasClaim(_claimName, _claimValue))
            {
                filterContext.Result = new ForbidResult();
            }
        }
    }
}
