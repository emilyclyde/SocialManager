using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using socialmanager.Extensions;
using socialmanager.Filters;
using socialmanager.Models;


namespace socialmanager.Controllers
{
    [Authorize]
    [FacebookAccessToken]
    public class FacebookController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("ExternalCallBack", "Facebook");
                return uriBuilder.Uri;
            }
        }

        private RedirectResult GetFacebookLoginURL()
        {

            if (Session["AccessTokenRetryCount"] == null ||
                (Session["AccessTokenRetryCount"] != null &&
                 Session["AccessTokenRetryCount"].ToString() == "0"))
            {
                Session.Add("AccessTokenRetryCount", "1");

                FacebookClient fb = new FacebookClient();
                fb.AppId = ConfigurationManager.AppSettings["Facebook_AppId"];
                return Redirect(fb.GetLoginUrl(new
                {
                    scope = ConfigurationManager.AppSettings["Facebook_Scope"],
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code"
                }).ToString());
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to obtain a valid Facebook Token, contact support";
                return Redirect(Url.Action("Index", "Error"));
            }
        }
        // GET: Facebook
        public async Task<ActionResult> Index()
        {
            var access_token = HttpContext.Items["access_token"].ToString();


            var appsecret_proof = access_token.GenerateAppSecretProof();

            var fb = new FacebookClient(access_token);

            //Get user's profile
            dynamic myInfo =
                await
                    fb.GetTaskAsync(
                        "me?fields= first_name, last_name, link, locale, email, name, birthday, gender, location, bio, age_range"
                            .GraphAPICall(appsecret_proof));
            //   var facebookProfile = DynamicExtension.ToStatic<Models.FacebookProfileViewModel>(myInfo);

            //Get user's picture
            dynamic profileImgResult =
                await
                    fb.GetTaskAsync(
                        "{0}/picture?width200&height200&redirect=false".GraphAPICall((string)myInfo.id,
                            appsecret_proof));
            //facebookProfile.ImageURL = profileImgResult.data.url;



            var facebookProfile = new FacebookProfileViewModel()
            {
                FirstName = myInfo.first_name,
                LastName = myInfo.last_name,
                ImageURL = profileImgResult.data.url,
                LinkURL = myInfo.link,
                Locale = myInfo.locale,
                email = myInfo.email,
                Fullname = myInfo.name,
                birthdate = System.DateTime.Parse(myInfo.birthday),
                gender = myInfo.gender,
                // age_range = myInfo.age_range,
                Location = myInfo.location.name,
                Bio = myInfo.bio,

            };

            return View(facebookProfile);

        }

    }
}


