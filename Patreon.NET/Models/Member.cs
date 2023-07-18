using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;

namespace Patreon.NET
{
    public class Member
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "campaign_lifetime_support_cents")]
        public int CampaignLifetimeSupportCents { get; set; }

        [JsonProperty(PropertyName = "currently_entitled_amount_cents")]
        public int EntitledAmountCents { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "is_follower")]
        public bool IsFollower { get; set; }

        [JsonProperty(PropertyName = "last_charge_date")]
        public DateTimeOffset? LastChargeDate { get; set; }

        [JsonProperty(PropertyName = "last_charge_status")]
        public string LastChargeStatus { get; set; }

        [JsonProperty(PropertyName = "lifetime_support_cents")]
        public int LifetimeSupportCents { get; set; }

        [JsonProperty(PropertyName = "next_charge_date")]
        public DateTimeOffset? NextChargeDate { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "patron_status")]
        public string PatreonStatus { get; set; }

        [JsonProperty(PropertyName = "pledge_cadence")]
        public int? PledgeCadence { get; set; }

        [JsonProperty(PropertyName = "pledge_relationship_start")]
        public DateTimeOffset? PledgeRelationshipStart { get; set; }

        [JsonProperty(PropertyName = "will_pay_amount_cents")]
        public int WillPayAmountCents { get; set; }

        [JsonProperty(PropertyName = "user")]
        public Relationship<User> User { get; set; }

        [JsonProperty(PropertyName = "pledge_history")]
        public Relationship<List<PledgeEvent>> PledgeHistory { get; set; }

        [JsonProperty(PropertyName = "currently_entitled_tiers")]
        public Relationship<List<Tier>> CurrentlyEntitledTiers { get; set; }
    }
}
