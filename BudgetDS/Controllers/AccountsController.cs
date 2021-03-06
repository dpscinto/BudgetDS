﻿using System;
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
using BudgetDS.Helpers;

namespace BudgetDS.Controllers
{
    [AuthorizeHouseholdRequired]
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            var user = Convert.ToInt32(User.Identity.GetHouseholdId());
            var accounts = db.Accounts.Where(a => a.HouseholdId == user);
            return View(accounts.ToList());
        }

        // GET: Accounts(Partial)
        public PartialViewResult _AccountBal(int? id)
        {
            var user = Convert.ToInt32(User.Identity.GetHouseholdId());
            var accounts = db.Accounts.Where(a => a.HouseholdId == user);
            return PartialView(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Balance,HouseholdId")] Account account)
        {
            var user = db.Users.Find(User.Identity.GetUserId());           

            if (ModelState.IsValid)
            {
                account.HouseholdId = (int)user.HouseholdId;
                db.Accounts.Add(account);
                db.SaveChanges();

                var hhid = Convert.ToInt32(User.Identity.GetHouseholdId());
                var house = db.Households.Find(hhid);
                var cat = house.Category.FirstOrDefault(c => c.Name == "Miscellaneous");

                Transaction initialTransaction = new Transaction
                {
                    Date = DateTime.Now,
                    Description = "Initial Balance",
                    Amount = account.Balance,
                    Category = cat,
                    Type = true,
                    Reconcile = true
                };

                account.Transactions.Add(initialTransaction);
                db.SaveChanges();

                return RedirectToAction("Index", "Accounts");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Balance,HouseholdId")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
