using EventStore.Client;

namespace illyW.Framework.Models;

public class AllRehydratorConfiguration
{
    public bool ResolveLinkTos { get; set; } = false;
    public SubscriptionFilterOptions FilterOptions { get; set; } = null;
}