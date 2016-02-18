using BudgetDS.Models;
using BudgetDS.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;

namespace BudgetDS.Helpers
{
    public class Helper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Household GetHousehold(string userId)
        {
            var user = db.Users.Find(userId);
            if(user == null)
            {
                return null;
            }
            if (user.HouseholdId == null)
            {
                return null;
            }
            var hh = db.Households.Find(user.HouseholdId);

            return hh;
        }
    }

    public static class Extension
    {
        public static string GetHouseholdId(this IIdentity user)
        {
            var ClaimUser = (ClaimsIdentity)user;
            var Claim = ClaimUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            if (Claim != null)
                return Claim.Value;
            else
                return null;
        }


        public static bool IsInHousehold(this IIdentity user)
        {
            var cUser = (ClaimsIdentity)user;
            var hid = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return (hid != null && !string.IsNullOrWhiteSpace(hid.Value));
        }

        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        {
            context.GetOwinContext().Authentication.SignOut();
            await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user, isPersistent: false, rememberBrowser: false);
        }
    }
}