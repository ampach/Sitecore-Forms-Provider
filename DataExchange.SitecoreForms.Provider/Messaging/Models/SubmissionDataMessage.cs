using System;
using Sitecore.ExperienceForms.Data.Entities;

namespace DataExchange.SitecoreForms.Provider.Messaging.Models
{
    public class SubmissionDataMessage
    {
        public FormEntry FormEntry { get; set; }
        public Guid BatchId { get; set; }
        public string ContactId { get; set; }
    }
}