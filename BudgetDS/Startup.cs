using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BudgetDS.Startup))]
namespace BudgetDS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
