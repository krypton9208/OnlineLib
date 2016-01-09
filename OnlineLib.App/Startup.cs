using System;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.DataProtection;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using OnlineLib.Repository.Repository;
using Owin;

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
            builder.RegisterType<LibRoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<LibSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();
            builder.RegisterType<LibraryRepository>().As<ILibraryRepository>().InstancePerRequest();
            builder.RegisterType<BooksRepository>().As<IBooksRepository>().InstancePerRequest();
            builder.RegisterType<EmailService>().As<IIdentityMessageService>().InstancePerRequest();
            builder.RegisterType<LoanActivityRepository>().As<ILoanActivityRepository>().InstancePerRequest();
            builder.RegisterType<LibManagerRepository>().As<ILibManagerRepository>().InstancePerRequest();
            // REGISTER CONTROLLERS
            // builder.RegisterControllers<AccountController>(typeof (ILibraryRepository)).AsSelf().InstancePerRequest();
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
