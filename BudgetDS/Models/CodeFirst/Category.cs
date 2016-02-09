using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetDS.Models.CodeFirst
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int HouseholdId { get; set; }
        public int CategoryListId { get; set; }

        public virtual Household Household { get; set; }
        public virtual CategoryList CategoryList { get; set; }
    }
}