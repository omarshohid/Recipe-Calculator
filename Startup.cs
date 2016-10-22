using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecipeCalculator.Startup))]
namespace RecipeCalculator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
