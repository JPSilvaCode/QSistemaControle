using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QSistemaControle.Startup))]
namespace QSistemaControle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
