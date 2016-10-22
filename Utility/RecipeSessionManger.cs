using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;


namespace RecipeCalculator.Utility
{

    public class SessionManger
    {

        public static tblRecipeUser LoggedInUser(HttpSessionStateBase session)
        {
            return (tblRecipeUser)session["LoggedInUser"];
        }

        public static void SetLoggedInUser(HttpSessionStateBase session, string recipeUser)
        {
            session["LoggedInUser"] = recipeUser;
        }


        public static DateTime LoggedInTime(HttpSessionStateBase session)
        {
            if (session["LoggedInTime"] != null)
            {
                return (DateTime)session["LoggedInTime"];
            }
            else
                return DateTime.Now;

        }

        public static void SetLoggedInTime(HttpSessionStateBase session, DateTime datetime)
        {
            session["LoggedInTime"] = datetime;
        }

        public static bool IsSessionNull(HttpSessionStateBase session)
        {
            if (session.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool isUserLoggedIn()
        {
            decimal? loggedIn = 0;
            string userId = HttpContext.Current.User.Identity.GetUserId();
            RecipeCalculatorEntities recipeEn = new RecipeCalculatorEntities();

            loggedIn = recipeEn.tblRecipeUsers.Where(a => a.UserId == userId).Select(a => a.is_loggedIn).FirstOrDefault();
            if (loggedIn == 1)
                return true;
            else
                return false;
        }

        public decimal? GetUserLoggedIn(string userId)
        {
            decimal? loggedIn = 0;
            RecipeCalculatorEntities recipeEn = new RecipeCalculatorEntities();

            loggedIn = recipeEn.tblRecipeUsers.Where(a => a.UserId == userId).Select(a => a.is_loggedIn).FirstOrDefault();
            return loggedIn;
        }

    }
}