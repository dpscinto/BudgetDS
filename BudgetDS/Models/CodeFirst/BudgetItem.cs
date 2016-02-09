using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetDS.Models.CodeFirst
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int HouseholdId { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public int Frequency { get; set; }
        [Required]
        public bool Type { get; set; }

        public virtual Household Household { get; set; }
        public virtual Category Category { get; set; }
    }
}