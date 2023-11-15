using System.Collections.Specialized;

namespace sample;

public class SolutionBuilderState
{
    public bool AzureAuthentication { get; set; } = false;
    public string ClientId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;

    public string SolutionName { get; set; } = string.Empty;

    public string WorkingDirectory { get; set; } = string.Empty;

    public List<string> ProjectNames { get; set; } = new();
}

