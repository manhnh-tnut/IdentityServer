using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.ApiResources.Models;

public class ApiResourcePropertiesDto
{
    public int ApiResourcePropertyId { get; set; }

    public int ApiResourceId { get; set; }

    public string ApiResourceName { get; set; }

    [Required]
    public string Key { get; set; }

    [Required]
    public string Value { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<ApiResourcePropertyDto> ApiResourceProperties { get; set; } = new();
}