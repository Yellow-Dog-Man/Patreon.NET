const string CAMPAIGN_ID = "CAMPAIGN_ID";
const string ACCESS_TOKEN = "ACCESS_TOKEN";

var patreon = new Patreon.NET.PatreonClient(CAMPAIGN_ID, ACCESS_TOKEN);

await patreon.UpdateCampaignInfo().ConfigureAwait(false);

await foreach (var member in patreon.GetCampaignMembers().ConfigureAwait(false))
{
    Console.WriteLine($"Member: {member.FullName} - {member.Email}");

    foreach(var tier in member.CurrentlyEntitledTiers.Data)
    {
        var tierData = patreon.TryGetTierById(tier.Id);

        Console.WriteLine($"\tEntitled Tier: {tierData.Title} - {tierData.AmountCents} cents ({tierData.Description})");
    }

    foreach(var pledge in member.PledgeHistory.Data)
    {
        var tier = patreon.TryGetTierById(pledge.TierId);

        Console.WriteLine($"\tPledge: {pledge.Id}, {pledge.Date}, Amount: {pledge.AmountCents * 0.01:F2} {pledge.CurrencyCode}, " +
            $"Payment Status: {pledge.PaymentStatus}\n" +
            $"\t\tTier {tier.Title}, USD: {tier.AmountCents * 0.01:F2}, URL: {tier.URL}");
    }
}

Console.Read();