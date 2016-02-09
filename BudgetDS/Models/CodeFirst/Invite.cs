using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDS.Models.CodeFirst
{
    public class Invite
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string PUser { get; set; }
        public int Code { get; set; }

        public virtual Household Household { get; set; }
    }
}