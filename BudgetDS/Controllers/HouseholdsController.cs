using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetDS.Models;
using BudgetDS.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System.Configuration;

namespace BudgetDS.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Households/Details(Index)/5
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Household household = db.Households.Find(user.HouseholdId);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            ViewBag.ErrorMessage = TempData["errorMessage"];
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                if (user.HouseholdId == null)
                {
                    db.Households.Add(household);
                    db.SaveChanges();

                    var hh = db.Households.OrderByDescending(h => h.Id).First();
                    user.HouseholdId = hh.Id;
                    db.SaveChanges();

                    return RedirectToAction("Index", new { id = hh.Id });
                }

                TempData["errorMessage"] = "Invalid Create Attempt";
                return RedirectToAction("Create", "Households");
            }

            return View(household);
        }

        // POST: Households/Join
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(string Code)
        {
            if (ModelState.IsValid)
            {
                var currUser = User.Identity.GetUserName();
                var invite = db.Invites.FirstOrDefault(c => c.Code == Code && c.PUser == currUser);
                if (invite != null)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    user.HouseholdId = invite.HouseholdId;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = user.HouseholdId });
                }
                TempData["errorMessage"] = "Invalid Join Code";
                return RedirectToAction("Create", "Households");
            }
            return View();
        }


        // POST: Households/Leave
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Leave(int? Id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseholdId = null;
            db.SaveChanges();

            return RedirectToAction("Create", "Households");
        }

        [HttpPost]
        public ActionResult Invite(SendInvite sendinvite)
        {
            var invite = new Invite();
            var keycode = SendInvite.GetUniqueKey(6);

            invite.PUser = sendinvite.Email;
            invite.HouseholdId = sendinvite.HouseholdId;
            invite.Code = keycode;
            db.Invites.Add(invite);
            db.SaveChanges();

            var Emailer = new EmailService();
            var mail = new IdentityMessage
            {
                Subject = "You're Invited to Join a Household on Budget Tracker!",
                Destination = sendinvite.Email,
                Body = "You've been invited to join a household on Budget Tracker. Here is your unique join code " + invite.Code + ". Click <a href='https://dscinto-budget.azurewebsites.net'>here</a> and enter your code to join the household."
            };
            Emailer.SendAsync(mail);

            return RedirectToAction("Index", "Households");
        }
    }
}
