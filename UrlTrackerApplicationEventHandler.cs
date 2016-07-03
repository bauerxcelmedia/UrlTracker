using InfoCaster.Umbraco.UrlTracker.Helpers;
using InfoCaster.Umbraco.UrlTracker.Repositories;
using System.Web;
using System.Web.UI;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Web.UI.Pages;

namespace InfoCaster.Umbraco.UrlTracker
{
    public class UrlTrackerApplicationEventHandler : ApplicationEventHandler
    {
        protected ClientTools ClientTools
        {
            get
            {
                Page page = HttpContext.Current.CurrentHandler as Page;
                if (page != null)
                    return new ClientTools(page);
                return null;
            }
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            if (!UrlTrackerSettings.IsDisabled && !UrlTrackerSettings.IsTrackingDisabled)
            {
                UrlTrackerRepository.ReloadForcedRedirectsCache();
                Domain.AfterDelete += Domain_AfterDelete;
                Domain.AfterSave += Domain_AfterSave;
                Domain.New += Domain_New;
            }
        }


        void Domain_New(Domain sender, NewEventArgs e)
        {
            UmbracoHelper.ClearDomains();
        }

        void Domain_AfterSave(Domain sender, SaveEventArgs e)
        {
            UmbracoHelper.ClearDomains();
        }

        void Domain_AfterDelete(Domain sender, umbraco.cms.businesslogic.DeleteEventArgs e)
        {
            UmbracoHelper.ClearDomains();
        }
    }
}