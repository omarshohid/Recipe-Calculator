using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using RecipeCalculator.Models;
using System.Data.Entity;


namespace RecipeCalculator.Utility
{

    public class SessionManger
    {
        public HttpResponse response;

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

        public static decimal? GetUserLoggedIn(string userId)
        {
            decimal? loggedIn = 0;
            RecipeCalculatorEntities recipeEn = new RecipeCalculatorEntities();

            loggedIn = recipeEn.tblRecipeUsers.Where(a => a.UserId == userId).Select(a => a.is_loggedIn).FirstOrDefault();
            return loggedIn;
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
        public class CheckUserSessionAttribute : ActionFilterAttribute
        {
            public string user_id { get; set; }

            //private IGenericFactory<tblBrokerUser> brokerUserFactory;
            public UserManager<ApplicationUser> UserManager { get; private set; }
            //private IGenericFactory<tblUserActionMapping> userActionMappingFactory;
            public string PropertyName { get; private set; }

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                //var user = session["BrokerOfLoggedInUser"];
                if ((session["LoggedInUser"] == null))
                {
                    if (GetUserLoggedIn(HttpContext.Current.User.Identity.GetUserId()) == 1)
                    {
                        RecipeCalculatorEntities rce = new RecipeCalculatorEntities();
                        tblRecipeUser ru = new tblRecipeUser();

                        //brokerUserFactory = new BrokerUserFactory();
                        //decimal membership_id = SessionManger.BrokerOfLoggedInUser(Session).membership_id;
                        string user_id = HttpContext.Current.User.Identity.GetUserId();
                        ru.is_loggedIn = 0;
                        ru.UserId = user_id;

                        rce.tblRecipeUsers.Attach(ru);
                        var entry = rce.Entry(ru);
                        entry.State = EntityState.Modified;
                        rce.SaveChanges();


                        filterContext.HttpContext.GetOwinContext().Authentication.SignOut();
                        session.RemoveAll();
                        session.Clear();
                        session.Abandon();
                    }

                    //send them off to the login page
                    var url = new UrlHelper(filterContext.RequestContext);
                    var loginUrl = url.Content("~/Account/Login");                  
                    filterContext.Result = new RedirectResult(loginUrl);
                    return;
                }


            }


        }
    }
}