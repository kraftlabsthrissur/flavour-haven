using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace TokenBaseAuthentication
{
    public class MyAuthProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            //UserEntities obj = new UserEntities();
            //var userdata = obj.EF_UserLogin(context.UserName, context.Password).FirstOrDefault();
            //var user = UserManager.FindByName(context.UserName);
            var userdata = "";
                //obj.EF_UserLogin(context.UserName, context.Password).FirstOrDefault();
            if (userdata != null)
            {
            //    identity.AddClaim(new Claim(ClaimTypes.Role, userdata.UserRole));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, userdata.UserName));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                context.Rejected();
            }
        }

    }

}