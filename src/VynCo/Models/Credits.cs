using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Credit balance information.</summary>
public class CreditBalance
{
    [JsonPropertyName("balance")] public int Balance { get; set; }
    [JsonPropertyName("monthlyCredits")] public long MonthlyCredits { get; set; }
    [JsonPropertyName("usedThisMonth")] public int UsedThisMonth { get; set; }
    [JsonPropertyName("tier")] public string Tier { get; set; } = "";
    [JsonPropertyName("overageRate")] public double OverageRate { get; set; }
}

/// <summary>Credit usage breakdown.</summary>
public class CreditUsage
{
    [JsonPropertyName("operations")] public List<UsageRow> Operations { get; set; } = new();
    [JsonPropertyName("total")] public long Total { get; set; }
    [JsonPropertyName("period")] public UsagePeriod Period { get; set; } = new();
}

/// <summary>Usage period boundaries.</summary>
public class UsagePeriod
{
    [JsonPropertyName("since")] public string Since { get; set; } = "";
    [JsonPropertyName("until")] public string Until { get; set; } = "";
}

/// <summary>Usage for a single operation type.</summary>
public class UsageRow
{
    [JsonPropertyName("operation")] public string Operation { get; set; } = "";
    [JsonPropertyName("count")] public long Count { get; set; }
    [JsonPropertyName("totalCredits")] public long TotalCredits { get; set; }
}

/// <summary>Credit ledger history.</summary>
public class CreditHistory
{
    [JsonPropertyName("items")] public List<CreditLedgerEntry> Items { get; set; } = new();
    [JsonPropertyName("total")] public long Total { get; set; }
}

/// <summary>A single credit ledger entry.</summary>
public class CreditLedgerEntry
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("entryType")] public string EntryType { get; set; } = "";
    [JsonPropertyName("amount")] public int Amount { get; set; }
    [JsonPropertyName("balance")] public int Balance { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = "";
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}
