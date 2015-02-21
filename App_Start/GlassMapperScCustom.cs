using System.Collections.Generic;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Verndale.SharedSource._Classes.Shared.Customizations.Customized_Glass;
using Castle.MicroKernel.Registration;


namespace Verndale.SharedSource.App_Start
{
    public static  class GlassMapperScCustom
    {
		public static void CastleConfig(IWindsorContainer container){
			var config = new Config();

            // added by Verndale for Fallback, 
            // must check FallbackItem in the FallbackCheckTask before determining to abort the glass process of getting the item
		    container.Register(
                Component.For<IObjectConstructionTask>().ImplementedBy<FallbackCheckTask>().LifestyleTransient()
                );

			container.Install(new SitecoreInstaller(config));
		}

        public static IConfigurationLoader[] GlassLoaders(){			
			
			/* USE THIS AREA TO ADD FLUENT CONFIGURATION LOADERS
             * 
             * If you are using Attribute Configuration or automapping/on-demand mapping you don't need to do anything!
             * 
             */

			return new IConfigurationLoader[]{};
		}
		public static void PostLoad(){
			//Remove the comments to activate CodeFist
			/* CODE FIRST START
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
             * CODE FIRST END
             */
		}
    }
}
