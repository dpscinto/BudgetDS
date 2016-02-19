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
using BudgetDS.Helpers;

namespace BudgetDS.Controllers
{
    [AuthorizeHouseholdRequired]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public PartialViewResult _TransIndex(int? id)
        {
            var user = Convert.ToInt32(User.Identity.GetHouseholdId());
            var transactions = db.Transactions.Where(t => t.Account.HouseholdId == user && t.AccountId == t.Account.Id);
            return PartialView(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create(int AccountId)
        {
            var trans = new Transaction()
            {
                AccountId = (int)AccountId
            };

            var Hhid = Convert.ToInt32(User.Identity.GetHouseholdId());
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.HouseholdId == Hhid), "Id", "Name");
            return View(trans);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,CategoryId,Date,Description,Type,Amount,Reconcile")] Transaction transaction, int AccountId)
        {
            var account = db.Accounts.FirstOrDefault(a => a.Id == AccountId);

            if (ModelState.IsValid)
            {
                if (transaction.Type == true)
                {
                    account.Balance += transaction.Amount;
                }
                else
                {
                    account.Balance -= transaction.Amount;
                }
                
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Details", "Accounts", new { id = AccountId });

            }
            var Hhid = Convert.ToInt32(User.Identity.GetHouseholdId());
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.HouseholdId == Hhid), "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Create(Partial)
        public PartialViewResult _AddTrans()
        {
            

            var Household = db.Households.Find(Convert.ToInt32(User.Identity.GetHouseholdId()));
            ViewBag.AccountId = new SelectList(Household.Account, "Id", "Name");
            ViewBag.CategoryId = new SelectList(Household.Category, "Id", "Name");
            return PartialView();
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            var Hhid = Convert.ToInt32(User.Identity.GetHouseholdId());
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c=>c.HouseholdId == Hhid), "Id", "Name");
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,CategoryId,Date,Description,Type,Amount,Reconcile")] Transaction transaction, int AccountId)
        {
            if (ModelState.IsValid)
            {
                var account = db.Accounts.FirstOrDefault(a => a.Id == AccountId);
                var original = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);

                if (original.Type == true)
                {
                    account.Balance -= original.Amount;
                }
                else
                {
                    account.Balance += original.Amount;
                }

                if (transaction.Type == true)
                {
                    account.Balance += transaction.Amount;
                }
                else
                {
                    account.Balance -= transaction.Amount;
                }

                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Accounts", new { id = transaction.AccountId });
            }

            var Hhid = Convert.ToInt32(User.Identity.GetHouseholdId());
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.HouseholdId == Hhid), "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            var account = db.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);

            if (transaction.Type == true)
            {
                account.Balance -= transaction.Amount;
            }
            else
            {
                account.Balance += transaction.Amount;
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Details", "Accounts", new { id = transaction.AccountId });
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
