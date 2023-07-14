const string CAMPAIGN_ID = "CAMPAIGN_ID";
const string ACCESS_TOKEN = "ACCESS_TOKEN";

var patreon = new Patreon.NET.PatreonClient(CAMPAIGN_ID, ACCESS_TOKEN);

var info = await patreon.GetCampaign().ConfigureAwait(false);

await foreach(var member in patreon.GetCampaignMembers().ConfigureAwait(false))
    Console.WriteLine(member.Attributes.FullName);

Console.Read();