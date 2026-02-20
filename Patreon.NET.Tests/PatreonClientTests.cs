using System.Net;

namespace Patreon.NET.Tests
{
    [TestClass]
    public sealed class PatreonClientTests
    {
        [TestMethod]
        public void TestConstructor()
        {
            var campaignId = "123456";
            var token = "TOKEN";

            var client = new PatreonClient(campaignId, token);

            CheckAuth(client, campaignId, token);
        }

        [TestMethod]
        public void TestUpdateSettings()
        {
            var campaignId = "123456";
            var token = "TOKEN";

            var client = new PatreonClient(campaignId, token);

            CheckAuth(client, campaignId, token);

            
            // Just updating the token.
            var newToken = "9001";
            client.UpdateSettings(newToken);
            CheckAuth(client, campaignId, newToken);

            // Updating both
            var newCampaign = "CHEESE";
            client.UpdateSettings(newToken, newCampaign);
            CheckAuth(client, newCampaign, newToken);
        }

        public void CheckAuth(PatreonClient client, string campaignId, string token)
        {
            Assert.AreEqual(campaignId, client.campaignId, "Campaign ID must match input");

            var auth = client.httpClient.DefaultRequestHeaders.Authorization;
            Assert.IsNotNull(auth, "Authorization header must be present");
            Assert.AreEqual(token, auth.Parameter, "Token must be present in request headers");
            Assert.AreEqual("Bearer", auth.Scheme);
        }
    }
}
