using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var session = (UserLogin)HttpContext.Current.Session[CommonContants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectResult("~/Admin/Login");               
                return;
            }
            
            List<string> privilegeLevels = this.GetListCredentialByUserName(session.UserName);
            // If they are authorized, handle accordingly
            if (privilegeLevels != null && privilegeLevels.Contains(this.RoleID) || session.GroupID.Equals("ADMIN"))
            {
                return;
            }
            else
            {
                // Otherwise redirect to your specific authorized area
                filterContext.Result = new RedirectResult("~/Admin/Error/NotFound");
            }
        }

        private List<string> GetListCredentialByUserName(string username){
            var credential = (List<string>)HttpContext.Current.Session[CommonContants.SESSION_CREDENTIALS];
            return credential;
        }

    }
}