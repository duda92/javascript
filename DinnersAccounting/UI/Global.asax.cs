﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using UI.Concrete;
using Microsoft.Practices.Unity;
using System.Security.Principal;
using System.Threading;
using DA.Dinners.Domain;
using DA.Dinners.Domain.Concrete;
using UI.App_Start;

namespace UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        //void ConfigureApi(HttpConfiguration config)
        //{
        //    config.DependencyResolver = new SimpleContainer();
        //}
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Bootstrapper.Initialise();

            //ConfigureApi(GlobalConfiguration.Configuration);

            Database.SetInitializer(new DinnersInitializer());

        }
    }
}