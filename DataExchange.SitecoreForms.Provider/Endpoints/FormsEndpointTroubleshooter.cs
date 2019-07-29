using System;
using System.Data.Common;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Troubleshooters;
using Sitecore.DependencyInjection;
using Sitecore.ExperienceForms.Data;

namespace DataExchange.SitecoreForms.Provider.Endpoints
{
    public class FormsEndpointTroubleshooter : BaseEndpointTroubleshooter
    {
        //private readonly IFormDataProvider _formDataProvider;

        //public FormsEndpointTroubleshooter(IFormDataProvider formDataProvider)
        //{
        //    this._formDataProvider = formDataProvider;
        //}

        protected override ITroubleshooterResult Troubleshoot(Endpoint endpoint, TroubleshooterContext context)
        {

            
            FormsSettings plugin = endpoint.GetPlugin<FormsSettings>();
            if (plugin == null)
                return (ITroubleshooterResult)TroubleshooterResult.FailResult(string.Format("Endpoint plugin is missing. Plugin type: {0}", (object)"FormsSettings"), (Exception)null);
            var formDataProvider = (IFormDataProvider)Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService(typeof(IFormDataProvider));
            if (formDataProvider == null)
                return (ITroubleshooterResult)TroubleshooterResult.FailResult(string.Format("Forms Data Provider is missing. Plugin type: {0}", (object)"FormsSettings"), (Exception)null);
            
            try
            {
                var results = formDataProvider.GetEntries(Guid.Empty,DateTime.MinValue, DateTime.MinValue);
                if (results.Count > 0)
                {
                    throw new Exception("Unexpected results.");
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.StackTrace);
                return (ITroubleshooterResult)TroubleshooterResult.FailResult(string.Format("{0} {1} {2}", (object)"Exception was thrown.", (object)"Read more in log file.", (object)ex.Message), ex);
            }
            finally
            {
                //dbConnection?.Dispose();
            }
            return (ITroubleshooterResult)TroubleshooterResult.SuccessResult("Database connection was successfully established.");
        }
    }
}
