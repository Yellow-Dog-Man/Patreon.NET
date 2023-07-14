const string CAMPAIGN_ID = "CAMPAIGN_ID";
const string ACCESS_TOKEN = "ACCESS_TOKEN";

var patreon = new Patreon.NET.PatreonClient(CAMPAIGN_ID, ACCESS_TOKEN);

await patreon.UpdateCampaignInfo().ConfigureAwait(false);

await foreach (var member in patreon.GetCampaignMembers().ConfigureAwait(false))
{
    Console.WriteLine($"Member: {member.Attributes.FullName} - {member.Attributes.Email}");

    foreach(var pledge in member.Relationships.PledgeHistory)
    {
        var tier = patreon.TryGetTierById(pledge.Attributes.TierId);

        Console.WriteLine($"\tPledge: {pledge.Id}, {pledge.Attributes.Date}, Amount: {pledge.Attributes.AmountCents} {pledge.Attributes.CurrencyCode}, " +
            $"Payment Status: {pledge.Attributes.PaymentStatus}\n" +
            $"\t\tTier {tier.Title}, Cents: {tier.AmountCents}, URL: {tier.URL}");
    }
    Console.WriteLine(member.Attributes.FullName);
}

Console.Read();