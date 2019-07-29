using Sitecore.ExperienceForms.Data.Entities;

namespace DataExchange.SitecoreForms.Provider.Models
{
    public class FormSubmissionEntry
    {
        public FormEntry FormEntry { get; set; }

        public FormSubmissionContext FormSubmissionContext { get; set; }
    }
}