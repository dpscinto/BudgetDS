namespace BudgetDS.Migrations
{
    using Models.CodeFirst;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BudgetDS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BudgetDS.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.CategoryLists.Any())
            {
                context.CategoryLists.AddRange(
                    new List<CategoryList> {
                new CategoryList { Name = "Automobile" },
                new CategoryList { Name = "Bank Charges" },
                new CategoryList { Name = "Charity" },
                new CategoryList { Name = "Childcare" },
                new CategoryList { Name = "Clothing" },
                new CategoryList { Name = "Credit Card Fees" },
                new CategoryList { Name = "Education" },
                new CategoryList { Name = "Events" },
                new CategoryList { Name = "Expense Reimbursements" },
                new CategoryList { Name = "Food" },
                new CategoryList { Name = "Gifts" },
                new CategoryList { Name = "Healthcare" },
                new CategoryList { Name = "Household" },
                new CategoryList { Name = "Insurance" },
                new CategoryList { Name = "Investment" },
                new CategoryList { Name = "Job Expenses" },
                new CategoryList { Name = "Leisure" },
                new CategoryList { Name = "Loans" },
                new CategoryList { Name = "Miscellaneous" },
                new CategoryList { Name = "Paycheck" },
                new CategoryList { Name = "Pet Care" },
                new CategoryList { Name = "Savings" },
                new CategoryList { Name = "Taxes" },
                new CategoryList { Name = "Uncategorized" },
                new CategoryList { Name = "Utilities" },
                new CategoryList { Name = "Vacation" }
                    });
            }
        }
    }
}
