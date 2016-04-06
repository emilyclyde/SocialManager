using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace socialmanager.Filters
{
    
        public class FacebookAccessTokenAttribute : ActionFilterAttribute
        {

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {

                ApplicationUserManager _userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if (_userManager != null)
                {
                    var claimsforUser = _userManager.GetClaimsAsync(filterContext.HttpContext.User.Identity.GetUserId());
                    var access_token = claimsforUser.Result.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;

                    if (filterContext.HttpContext.Items.Contains("access_token"))
                        filterContext.HttpContext.Items["access_token"] = access_token;
                    else
                        filterContext.HttpContext.Items.Add("access_token", access_token);
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
