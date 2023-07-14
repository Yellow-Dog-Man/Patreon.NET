using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using JsonApiSerializer;
using JsonApiSerializer.JsonApi;
using System.Reflection;

namespace Patreon.NET
{
    public class PatreonClient : IDisposable
    {
        public const string SAFE_ROOT = "https://www.patreon.com/api/oauth2/v2/";
        public const string PUBLIC_ROOT = "https://www.patreon.com/api/";

        public static string CampaignURL(string campaignId) => SAFE_ROOT + $"campaigns/{campaignId}";
        public static string PledgesURL(string campaignId) => CampaignURL(campaignId) + "/pledges";
        public static string MembersURL(string campaignId) => CampaignURL(campaignId) + "/members";
        public static string MemberURL(string memberId) => SAFE_ROOT + $"members/{memberId}";
        public static string UserURL(string userId) => PUBLIC_ROOT + $"user/{userId}";

        HttpClient httpClient;
        string campaignId;

        public Campaign Campaign => _campaign;

        Campaign _campaign;
        Dictionary<string, Tier> _tiers;

        public PatreonClient(string campaignId, string accessToken)
        {
            this.campaignId = campaignId;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        static string GenerateFieldsAndIncludes(Type rootType, HashSet<Type> ignoreFields = null)
        {
            var str = new StringBuilder();
            var generatedTypes = new HashSet<Type>();
            var includes = new HashSet<string>();

            GenerateFieldsAndIncludes(rootType, str, generatedTypes, includes, ignoreFields);

            if (includes.Count > 0)
                str.Append("&include=" + string.Join(",", includes));

            return str.ToString();
        }

        static void GenerateFieldsAndIncludes(Type rootType, StringBuilder str, HashSet<Type> generatedTypes, HashSet<string> includes,
            HashSet<Type> ignoreFields)
        {
            if (!generatedTypes.Add(rootType))
                return;

            bool generateFields = ignoreFields == null || !ignoreFields.Contains(rootType);

            if (generateFields)
            {
                str.Append("fields%5B");

                var name = rootType.Name.Replace("Attributes", "");

                for (int i = 0; i < name.Length; i++)
                {
                    var ch = name[i];

                    if (char.IsUpper(ch) && i != 0)
                        str.Append("_");

                    str.Append(char.ToLower(ch));
                }

                str.Append("%5D=");
            }

            var relationships = new List<PropertyInfo>();

            foreach (var property in rootType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attribute = property.GetCustomAttribute<JsonPropertyAttribute>(true);

                if (attribute == null)
                    continue;

                var fieldName = attribute.PropertyName;

                if (fieldName == null ||
                    fieldName == "type" ||
                    fieldName == "id")
                    continue;

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Relationship<>))
                {
                    var type = ExtractRelationshipType(property.PropertyType);

                    if (generatedTypes.Contains(type))
                        continue;

                    // add it to the list of includes and handle generating fields afterwards
                    if(includes.Add(fieldName))
                        relationships.Add(property);

                    continue;
                }

                if (generateFields)
                {
                    str.Append(fieldName);
                    str.Append(",");
                }
            }

            // remove the last comma
            if(generateFields)
                str.Length -= 1;

            // handle the relationships now
            foreach (var relationship in relationships)
            {
                var relationshipType = ExtractRelationshipType(relationship.PropertyType);

                if(ignoreFields == null || !ignoreFields.Contains(relationshipType))
                    str.Append("&");

                GenerateFieldsAndIncludes(relationshipType, str, generatedTypes, includes, ignoreFields);
            }
        }

        static Type ExtractRelationshipType(Type type)
        {
            var embeddedType = type.GetGenericArguments()[0];

            if (embeddedType.IsGenericType &&
                (embeddedType.GetGenericTypeDefinition() == typeof(List<>) ||
                embeddedType.GetGenericTypeDefinition() == typeof(HashSet<>)))
                return embeddedType.GetGenericArguments()[0];

            return embeddedType;
        }

        public static string AppendQuery(string url, string query)
        {
            if (url.Contains("?"))
                url += "&" + query;
            else
                url += "?" + query;

            return url;
        }

        public async Task<HttpResponseMessage> GET(string url) => await httpClient.GetAsync(url).ConfigureAwait(false);

        public async Task<T> GET<T>(string url)
            where T : class
        {
            var response = await GET(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var settings = new JsonApiSerializerSettings();
                    return JsonConvert.DeserializeObject<T>(json, settings);
                }
                catch(Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex.ToString());
#endif
                }
            }

            return null;
        }

        public async Task<bool> UpdateCampaignInfo()
        {
            var url = CampaignURL(campaignId);

            url = AppendQuery(url, GenerateFieldsAndIncludes(typeof(Campaign)));

            var document = await GET<DocumentRoot<Campaign>>(url).ConfigureAwait(false);

            if (document?.Data == null)
                return false;

            _campaign = document.Data;

            _tiers = new Dictionary<string, Tier>();

            foreach (var tier in _campaign.Tiers.Data)
                if(tier.Id != null)
                    _tiers.Add(tier.Id, tier);

            return true;
        }

        public async IAsyncEnumerable<Member> GetCampaignMembers()
        {
            string next = MembersURL(campaignId);

            do
            {
                var url = next;

                url = AppendQuery(url, GenerateFieldsAndIncludes(typeof(Member), 
                    new HashSet<Type>() { typeof(PledgeEvent), typeof(Tier) }));

                var document = await GET<DocumentRoot<Member[]>>(url).ConfigureAwait(false);

                foreach (var d in document.Data)
                    yield return d;

                if (document.Links != null && document.Links.ContainsKey("next"))
                    next = document.Links["next"].Href;
                else
                    next = null;

            } while (next != null);
        }

        public async Task<User> GetUser(string id) => (await GET<UserData>(UserURL(id)).ConfigureAwait(false))?.User;

        public Tier TryGetTierById(string id)
        {
            if (_tiers == null)
                return null;

            if (_tiers.TryGetValue(id, out var tier))
                return tier;

            return null;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
