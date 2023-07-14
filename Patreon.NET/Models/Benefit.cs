using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patreon.NET
{
    public class Benefit
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "attributes")]
        public BenefitAttributes Attributes { get; set; }

        [JsonProperty(PropertyName = "relationships")]
        public BenefitRelationships Relationships { get; set; }
    }

    public class BenefitAttributes
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "app_external_id")]
        public string AppExternalId { get; set; }

        [JsonProperty(PropertyName = "app_meta")]
        public string AppMeta { get; set; }

        [JsonProperty(PropertyName = "benefit_type")]
        public string BenefitType { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "deliverables_due_today_count")]
        public int DeliverablesDueTodayCount { get; set; }

        [JsonProperty(PropertyName = "delivered_deliverables_count")]
        public int DeliveredDeliverablesCount { get; set; }

        [JsonProperty(PropertyName = "not_delivered_deliverables_count")]
        public int NotDeliveredDeliverablesCount { get; set; }

        [JsonProperty(PropertyName = "next_deliverable_due_date")]
        public DateTime NextDeliverableDueDate { get; set; }

        [JsonProperty(PropertyName = "is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty(PropertyName = "is_published")]
        public bool IsPublished { get; set; }

        [JsonProperty(PropertyName = "rule_type")]
        public string RuleType { get; set; }

        [JsonProperty(PropertyName = "tiers_count")]
        public int TiersCount { get; set; }
    }

    public class BenefitRelationships
    {
        [JsonProperty(PropertyName = "campaign")]
        public Campaign Campaign { get; set; }

        [JsonProperty(PropertyName = "tiers")]
        public List<Tier> Tiers { get; set; }
    }
}
