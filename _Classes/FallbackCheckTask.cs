using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Managers;
using Sitecore.Data.Items;
using Sitecore.SharedSource.PartialLanguageFallback.Extensions;

namespace Verndale.SharedSource._Classes.Shared.Customizations.Customized_Glass
{
    public class FallbackCheckTask : IObjectConstructionTask
    {
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null)
            {
                var scContext = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;

                // if the item itself is null, regardless of version, abort
                if (scContext.Item == null)
                {
                    args.AbortPipeline();
                    return;
                }
                
                // we could be trying to convert rendering parameters to a glass model, and if so, just return.
                if (String.Compare(scContext.Item.Paths.FullPath, "[orphan]/renderingParameters", true) == 0)
                {
                    return;
                }

                // the default glassmapper code would simply abort pipeline if the context items version count for the current langauge was 0
                // but this does not take item fallback into account
                // added here a check on the fallback extension method GetFallbackItem, recursively (for chained fallback)
                // and then if that fallback item is null or it's version count is 0 (and only then) would you go ahead and abort the pipeline
                if (scContext.Item.Versions.Count == 0)
                {
                    var fallBackItem = CheckRecursivelyForFallbackItem(scContext.Item);
                    if (fallBackItem == null)
                        args.AbortPipeline();
                    else if (fallBackItem.Versions.Count == 0)
                        args.AbortPipeline();
                    return;
                }
            }
        }

        // in the case of chained fallback, eg fr-CA -> en-CA -> en
        // could be that the middle languages don't have versions either, but DO have a fallback item
        // therefore, msut check back further until either a version is found, or there are no more fallback items
        private Item CheckRecursivelyForFallbackItem(Item thisItem)
        {
            var fallBackItem = thisItem.GetFallbackItem();
            if (fallBackItem != null)
            {
                if (fallBackItem.Versions.Count == 0)
                    fallBackItem = CheckRecursivelyForFallbackItem(fallBackItem);
            }
            return fallBackItem;
        }
    }
}