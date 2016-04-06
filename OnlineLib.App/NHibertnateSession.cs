using NHibernate;
using NHibernate.Cfg;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLib.Models
{
    public class NHibertnateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var employeeConfigurationFile = System.Web.Hosting.HostingEnvironment.MapPath(@"~\..\OnlineLib.Models\Nhibernate\Book.hbm.xml");
            configuration.AddFile(employeeConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
