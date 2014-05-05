using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YQ.Web.DB;
using YQ.Web.Models;
using System.Data.Common;

namespace YQ.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.CreateMap<EvalQuestionVM, EvalQuestion>();
            Mapper.CreateMap<EvalFormVM, EvalForm>();
            Mapper.CreateMap<EvalInstanceVM, EvalInstance>();

            Database.SetInitializer(new EvalDatabaseInitializer());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }
    }
}
