using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetDS.Models.CodeFirst
{
    public class Account
    {
        public Account()
        {
            this.Transaction = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}