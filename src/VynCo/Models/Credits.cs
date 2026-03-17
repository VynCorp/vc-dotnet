using System.Text.Json.Serialization;

namespace VynCo.Models;

public class CreditBalance
{
    [JsonPropertyName("balance")] public int Balance { get; set; }
    [JsonPropertyName("monthlyCredits")] public int MonthlyCredits { get; set; }
    [JsonPropertyName("usedThisMonth")] public int UsedThisMonth { get; set; }
    [JsonPropertyName("tier")] public string Tier { get; set; } = "";
    [JsonPropertyName("overageRate")] public decimal OverageRate { get; set; }
}

public class UsageBreakdown
{
    [JsonPropertyName("operations")] public List<OperationUsage> Operations { get; set; } = new();
    [JsonPropertyName("totalDebited")] public int TotalDebited { get; set; }
    [JsonPropertyName("period")] public string Period { get; set; } = "";
}

public class OperationUsage
{
    [JsonPropertyName("operation")] public string Operation { get; set; } = "";
    [JsonPropertyName("credits")] public int Credits { get; set; }
}

public class CreditLedgerEntry
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("teamId")] public Guid TeamId { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = "";
    [JsonPropertyName("amount")] public int Amount { get; set; }
    [JsonPropertyName("balanceAfter")] public int BalanceAfter { get; set; }
    [JsonPropertyName("operation")] public string? Operation { get; set; }
    [JsonPropertyName("referenceId")] public string? ReferenceId { get; set; }
    [JsonPropertyName("apiKeyId")] public Guid? ApiKeyId { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
}

/// <summary>Parameters for listing credit usage.</summary>
public class CreditUsageParams
{
    public string? Since { get; set; }
}

/// <summary>Parameters for listing credit history.</summary>
public class CreditHistoryParams
{
    public int Limit { get; set; } = 25;
    public int Offset { get; set; } = 0;
}
