using IdentityServer.Features.Shared.Resources.Interfaces;
using IdentityServer.Features.ApiResources.Mappers;
using IdentityServer.Features.ApiResources.Models;
using IdentityServer.Features.ApiResources.Services.Interfaces;
using IdentityServer.Infrastructure.Constants;
using IdentityServer.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Features.ApiResources.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(ControllerExceptionFilterAttribute))]
[Produces("application/json", "application/problem+json")]
[Authorize(Policy = AuthorizationConstants.AdministrationPolicy)]
public class ApiResourcesController : ControllerBase
{
    private readonly IApiResourceService _apiResourceService;
    private readonly IApiErrorResources _errorResources;

    public ApiResourcesController(IApiResourceService apiResourceService, IApiErrorResources errorResources)
    {
        _apiResourceService = apiResourceService;
        _errorResources = errorResources;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResourcesViewModel>> Get(string searchText, int page = 1, int pageSize = 10)
    {
        var apiResourcesDto = await _apiResourceService.GetApiResourcesAsync(searchText, page, pageSize);
        var ApiResourcesViewModel = apiResourcesDto.ToApiResourceViewModel<ApiResourcesViewModel>();

        return Ok(ApiResourcesViewModel);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResourceViewModel>> Get(int id)
    {
        var apiResourceDto = await _apiResourceService.GetApiResourceAsync(id);
        var ApiResourceViewModel = apiResourceDto.ToApiResourceViewModel<ApiResourceViewModel>();

        return Ok(ApiResourceViewModel);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] ApiResourceViewModel apiResourceApi)
    {
        var apiResourceDto = apiResourceApi.ToApiResourceViewModel<ApiResourceDto>();

        if (!apiResourceDto.Id.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var apiResourceId = await _apiResourceService.AddApiResourceAsync(apiResourceDto);
        apiResourceApi.Id = apiResourceId;

        return CreatedAtAction(nameof(Get), new { id = apiResourceId }, apiResourceApi);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] ApiResourceViewModel apiResourceApi)
    {
        var apiResourceDto = apiResourceApi.ToApiResourceViewModel<ApiResourceDto>();

        await _apiResourceService.GetApiResourceAsync(apiResourceDto.Id);
        await _apiResourceService.UpdateApiResourceAsync(apiResourceDto);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var apiResourceDto = new ApiResourceDto { Id = id };

        await _apiResourceService.GetApiResourceAsync(apiResourceDto.Id);
        await _apiResourceService.DeleteApiResourceAsync(apiResourceDto);

        return Ok();
    }

    [HttpGet("{id}/Secrets")]
    public async Task<ActionResult<ApiSecretsViewModel>> GetSecrets(int id, int page = 1, int pageSize = 10)
    {
        var apiSecretsDto = await _apiResourceService.GetApiSecretsAsync(id, page, pageSize);
        var apiSecretsApiDto = apiSecretsDto.ToApiResourceViewModel<ApiSecretsViewModel>();

        return Ok(apiSecretsApiDto);
    }

    [HttpGet("Secrets/{secretId}")]
    public async Task<ActionResult<ApiSecretViewModel>> GetSecret(int secretId)
    {
        var apiSecretsDto = await _apiResourceService.GetApiSecretAsync(secretId);
        var apiSecretApiDto = apiSecretsDto.ToApiResourceViewModel<ApiSecretViewModel>();

        return Ok(apiSecretApiDto);
    }

    [HttpPost("{id}/Secrets")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostSecret(int id, [FromBody] ApiSecretViewModel clientSecretApi)
    {
        var secretsDto = clientSecretApi.ToApiResourceViewModel<ApiSecretsDto>();
        secretsDto.ApiResourceId = id;

        if (!secretsDto.ApiSecretId.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var secretId = await _apiResourceService.AddApiSecretAsync(secretsDto);
        clientSecretApi.Id = secretId;

        return CreatedAtAction(nameof(GetSecret), new { secretId }, clientSecretApi);
    }

    [HttpDelete("Secrets/{secretId}")]
    public async Task<IActionResult> DeleteSecret(int secretId)
    {
        var apiSecret = new ApiSecretsDto { ApiSecretId = secretId };

        await _apiResourceService.GetApiSecretAsync(apiSecret.ApiSecretId);
        await _apiResourceService.DeleteApiSecretAsync(apiSecret);

        return Ok();
    }

    [HttpGet("{id}/Properties")]
    public async Task<ActionResult<ApiResourcePropertiesViewModel>> GetProperties(int id, int page = 1, int pageSize = 10)
    {
        var apiResourcePropertiesDto = await _apiResourceService.GetApiResourcePropertiesAsync(id, page, pageSize);
        var apiResourcePropertiesApiDto = apiResourcePropertiesDto.ToApiResourceViewModel<ApiResourcePropertiesViewModel>();

        return Ok(apiResourcePropertiesApiDto);
    }

    [HttpGet("Properties/{propertyId}")]
    public async Task<ActionResult<ApiResourcePropertyViewModel>> GetProperty(int propertyId)
    {
        var apiResourcePropertiesDto = await _apiResourceService.GetApiResourcePropertyAsync(propertyId);
        var apiResourcePropertyApiDto = apiResourcePropertiesDto.ToApiResourceViewModel<ApiResourcePropertyViewModel>();

        return Ok(apiResourcePropertyApiDto);
    }

    [HttpPost("{id}/Properties")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostProperty(int id, [FromBody] ApiResourcePropertyViewModel apiPropertyApi)
    {
        var apiResourcePropertiesDto = apiPropertyApi.ToApiResourceViewModel<ApiResourcePropertiesDto>();
        apiResourcePropertiesDto.ApiResourceId = id;

        if (!apiResourcePropertiesDto.ApiResourcePropertyId.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var propertyId = await _apiResourceService.AddApiResourcePropertyAsync(apiResourcePropertiesDto);
        apiPropertyApi.Id = propertyId;

        return CreatedAtAction(nameof(GetProperty), new { propertyId }, apiPropertyApi);
    }

    [HttpDelete("Properties/{propertyId}")]
    public async Task<IActionResult> DeleteProperty(int propertyId)
    {
        var apiResourceProperty = new ApiResourcePropertiesDto { ApiResourcePropertyId = propertyId };

        await _apiResourceService.GetApiResourcePropertyAsync(apiResourceProperty.ApiResourcePropertyId);
        await _apiResourceService.DeleteApiResourcePropertyAsync(apiResourceProperty);

        return Ok();
    }
}