using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetDS.Models.CodeFirst
{
    public class Category
    {
        public Category()
        {
        this.BudgetItem = new HashSet<BudgetItem>();
        this.Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<BudgetItem> BudgetItem { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
