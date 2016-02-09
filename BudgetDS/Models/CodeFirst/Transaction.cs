using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetDS.Models.CodeFirst
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        public DateTimeOffset Created { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Type { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public bool Reconcile { get; set; }

        public virtual Category Category { get; set; }
        public virtual Account Account { get; set; }

    }
}