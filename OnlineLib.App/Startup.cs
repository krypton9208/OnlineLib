using System;
using System.IO.MemoryMappedFiles;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using OnlineLib.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineLib.App.Startup))]
namespace OnlineLib.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // REGISTER DEPENDENCIES
            builder.RegisterType<OnlineLibDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<LibRoleStore>().As<IRoleStore<LibRole, Guid>>().InstancePerRequest();
            builder.RegisterType<LibUserStore>().As<IUserStore<LibUser, Guid>>().InstancePerRequest();
            builder.RegisterType<LibUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<LinRoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<LibSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
            builder.RegisterType<EmailService>().AsSelf().InstancePerRequest();

            // REGISTER CONTROLLERS
            builder.RegisterControllers(typeof (MvcApplication).Assembly);

            // BUILD CONTAINER
            var container = builder.Build();

            // SET AUTOFAC DEPENDENCY RESOLVER
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // REGISTER AUTOFAC WITH OWIN
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);

           
        }
    }
}
