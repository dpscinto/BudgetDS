using BudgetDS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BudgetDS.Helpers;
using Newtonsoft.Json;
using BudgetDS.Models.CodeFirst;

namespace BudgetDS.Controllers
{
    [AuthorizeHouseholdRequired]
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {

            Helper helper = new Helper();
            var hh = helper.GetHousehold(User.Identity.GetUserId());
            if (hh == null)
            {

            }
            return View();
        }

        public ActionResult GetChart()
        {

            var house = db.Households.Find(Convert.ToInt32(User.Identity.GetHouseholdId()));
            var donutData = (from cat in house.Category
                             let sum = (from bi in cat.BudgetItem
                                        where bi.Type == false 
                                        && bi.Amount > 0
                                        select bi.Amount).DefaultIfEmpty().Sum()
                             where sum > 0
                             select new
                           {
                               label = cat.Name,
                               value = sum
                           }).ToArray();

            return Content(JsonConvert.SerializeObject(donutData), "application/json");

        }

        public ActionResult GetChart2()
        {
            var house = db.Households.Find(Convert.ToInt32(User.Identity.GetHouseholdId()));
            var donutData2 = (from cat in house.Category
                              let sum = (from tr in cat.Transaction
                                    where tr.Type == false
                                    && tr.Amount > 0
                                    && tr.Date.Year == DateTime.Now.Year
                                    && tr.Date.Month == DateTime.Now.Month
                                    select tr.Amount).DefaultIfEmpty().Sum()
                             where sum > 0
                             select new
                             {
                                 label = cat.Name,
                                 value = sum
                             }).ToArray();

            return Content(JsonConvert.SerializeObject(donutData2), "application/json");

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}