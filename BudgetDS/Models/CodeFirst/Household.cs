using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetDS.Models.CodeFirst
{
    public class Household
    {
        public Household()
        {
            this.Account = new HashSet<Account>();
            this.Category = new HashSet<Category>();
            this.BudgetItem = new HashSet<BudgetItem>();
            this.Invite = new HashSet<Invite>();
            this.ApplicationUser = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<BudgetItem> BudgetItem { get; set; }
        public virtual ICollection<Invite> Invite { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}