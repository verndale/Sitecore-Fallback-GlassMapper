using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.ContentSearch.SolrProvider.CastleWindsorIntegration;
using Sitecore.Diagnostics;
using Glass.Mapper.Sc;

namespace yoursite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    
    // For Lucene
    //public class MvcApplication : Sitecore.Web.Application
    // For Solr
    //public class MvcApplication : WindsorApplication

    public class MvcApplication : WindsorApplication
    {
        
        // For Lucene
        //protected void Application_Start()
        // For Solr
        //public override void Application_Start()

        public override void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // START: For Solr ONLY, comment out if for Lucene 
            try
            {
                base.Application_Start();
            }
            catch (Exception e)
            {
                Log.Error("Unable to connect to SOLR", e, this);
            }
            // END: For Solr ONLY, comment out if for Lucene 
        }

        protected void Session_Start()
        {
            
        }

        protected void Application_BeginRequest()
        {
            // added for Fallback purposes.  Don't want to automatically check version count without first checking the fallbackitem existence
            // this is done with an addition to the GlassMapperScCustom.cs class to register FallbackCheckTask
            // and then the addition of that FallbackCheckTask class method to check GetFallbackItem
            Sitecore.Context.Items["Disable"] = new VersionCountDisabler();
        }
    }
}