const string CAMPAIGN_ID = "10789880";
const string ACCESS_TOKEN = "4P3KVEo1MaAIL_VJDaLBmQ_Jy0UL_yFe6XT-gJu_bhM";

var patreon = new Patreon.NET.PatreonClient(CAMPAIGN_ID, ACCESS_TOKEN);

await patreon.UpdateCampaignInfo().ConfigureAwait(false);

await foreach (var member in patreon.GetCampaignMembers().ConfigureAwait(false))
{
    Console.WriteLine($"Member: {member.FullName} - {member.Email}");

    foreach(var pledge in member.PledgeHistory.Data)
    {
        var tier = patreon.TryGetTierById(pledge.TierId);

        Console.WriteLine($"\tPledge: {pledge.Id}, {pledge.Date}, Amount: {pledge.AmountCents} {pledge.CurrencyCode}, " +
            $"Payment Status: {pledge.PaymentStatus}\n" +
            $"\t\tTier {tier.Title}, Cents: {tier.AmountCents}, URL: {tier.URL}");
    }
}

Console.Read();