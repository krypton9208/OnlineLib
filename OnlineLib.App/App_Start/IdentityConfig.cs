using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using OnlineLib.App;
using OnlineLib.Models;
namespace OnlineLib.App
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var mes = new MailMessage();
            mes.To.Add(message.Destination); // replace with valid value 
            mes.From = new MailAddress("register@onlinelib.pl"); // replace with valid value
            mes.Subject = message.Subject;
            mes.Body = message.Body;
            mes.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "register@onlinelib.pl", // replace with valid value
                    Password = "$MAB^1PjUs2f" // replace with valid value,

                };
                smtp.Host = "smtp.webio.pl";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 500000000;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                return smtp.SendMailAsync(mes);
            }
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class LibUserStore : UserStore<LibUser, LibRole, Guid, LibLogin, LibUserRole, LibClaim>
    {
        public LibUserStore(OnlineLibDbContext context)
            : base(context)
        {
        }
    }
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class LibUserManager : UserManager<LibUser, Guid>
    {

        public LibUserManager(IUserStore<LibUser, Guid> store, IDataProtectionProvider dataProtectionProvider)
            : base(store)
        {


            //public static LibUserManager Create(IdentityFactoryOptions<LibUserManager> options, IOwinContext context) 
            //{
            //var manager = new LibUserManager(new LibUserStore<LibUser, Guid>(context.Get<OnlineLibDbContext>()));
            // Configure validation logic for usernames
            UserValidator = new UserValidator<LibUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;
            EmailService = new EmailService();
            if (dataProtectionProvider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<LibUser, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
        

    }




    // Configure the application sign-in manager which is used in this application.
    public class LibSignInManager : SignInManager<LibUser, Guid>
    {
        public LibSignInManager(LibUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(LibUser user)
        {
            return user.GenerateUserIdentityAsync((LibUserManager)UserManager);
        }

        public static LibSignInManager Create(IdentityFactoryOptions<LibSignInManager> options, IOwinContext context)
        {
            return new LibSignInManager(context.GetUserManager<LibUserManager>(), context.Authentication);
        }

    }
}
