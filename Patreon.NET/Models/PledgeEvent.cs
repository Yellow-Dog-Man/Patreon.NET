using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;

namespace Patreon.NET
{
    public class PledgeEvent
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty(PropertyName = "currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty(PropertyName = "payment_status")]
        public string PaymentStatus { get; set; }

        [JsonProperty(PropertyName = "pledge_payment_status")]
        public string PledgePaymentStatus { get; set; }

        [JsonProperty(PropertyName = "tier_id")]
        public string TierId { get; set; }

        [JsonProperty(PropertyName = "tier_title")]
        public string TierTitle { get; set; }

        [JsonProperty(PropertyName = "tier")]
        public Relationship<Tier> Tier { get; set; }
    }
}
