const string CAMPAIGN_ID = "CAMPAIGN_ID";
const string ACCESS_TOKEN = "ACCESS_TOKEN";

var patreon = new Patreon.NET.PatreonClient(ACCESS_TOKEN);
//var pledges = await patreon.GetCampaignPledges(CAMPAIGN_ID);
await foreach(var member in patreon.GetCampaignMembers(CAMPAIGN_ID))
{
    Console.WriteLine(member);
}

Console.Read();