﻿using BudgetDS.Models;
using BudgetDS.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}