using IdentityServer.Features.Shared.Resources.Interfaces;
using IdentityServer.Features.ApiScopes.Services.Interfaces;
using IdentityServer.Features.ApiScopes.Mappers;
using IdentityServer.Features.ApiScopes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Features.ApiScopes.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(Infrastructure.Filters.ControllerExceptionFilterAttribute))]
[Produces("application/json", "application/problem+json")]
[Authorize(Policy = Infrastructure.Constants.AuthorizationConstants.AdministrationPolicy)]
public class ApiScopesController : ControllerBase
{
    private readonly IApiErrorResources _errorResources;
    private readonly IApiScopeService _apiScopeService;

    public ApiScopesController(IApiErrorResources errorResources, IApiScopeService apiScopeService)
    {
        _errorResources = errorResources;
        _apiScopeService = apiScopeService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiScopesViewModel>> GetScopes(string search, int page = 1, int pageSize = 10)
    {
        var apiScopesDto = await _apiScopeService.GetApiScopesAsync(search, page, pageSize);
        var apiScopesApiDto = apiScopesDto.ToApiScopeViewModel<ApiScopesViewModel>();

        return Ok(apiScopesApiDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiScopeViewModel>> GetScope(int id)
    {
        var apiScopesDto = await _apiScopeService.GetApiScopeAsync(id);
        var apiScopeApiDto = apiScopesDto.ToApiScopeViewModel<ApiScopeViewModel>();

        return Ok(apiScopeApiDto);
    }

    [HttpGet("{id}/Properties")]
    public async Task<ActionResult<ApiScopePropertiesViewModel>> GetScopeProperties(int id, int page = 1, int pageSize = 10)
    {
        var apiScopePropertiesDto = await _apiScopeService.GetApiScopePropertiesAsync(id, page, pageSize);
        var apiScopePropertiesApiDto = apiScopePropertiesDto.ToApiScopeViewModel<ApiScopePropertiesViewModel>();

        return Ok(apiScopePropertiesApiDto);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostScope([FromBody] ApiScopeViewModel apiScopeApi)
    {
        var apiScope = apiScopeApi.ToApiScopeViewModel<ApiScopeDto>();

        if (!apiScope.Id.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var id = await _apiScopeService.AddApiScopeAsync(apiScope);
        apiScope.Id = id;

        return CreatedAtAction(nameof(GetScope), new { scopeId = id }, apiScope);
    }

    [HttpPost("{id}/Properties")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostProperty(int id, [FromBody] ApiScopePropertyViewModel apiScopePropertyApi)
    {
        var apiResourcePropertiesDto = apiScopePropertyApi.ToApiScopeViewModel<ApiScopePropertiesDto>();
        apiResourcePropertiesDto.ApiScopeId = id;

        if (!apiResourcePropertiesDto.ApiScopePropertyId.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var propertyId = await _apiScopeService.AddApiScopePropertyAsync(apiResourcePropertiesDto);
        apiScopePropertyApi.Id = propertyId;

        return CreatedAtAction(nameof(GetProperty), new { propertyId }, apiScopePropertyApi);
    }

    [HttpGet("Properties/{propertyId}")]
    public async Task<ActionResult<ApiScopePropertyViewModel>> GetProperty(int propertyId)
    {
        var apiScopePropertyAsync = await _apiScopeService.GetApiScopePropertyAsync(propertyId);
        var resourcePropertyApiDto = apiScopePropertyAsync.ToApiScopeViewModel<ApiScopePropertyViewModel>();

        return Ok(resourcePropertyApiDto);
    }

    [HttpDelete("Properties/{propertyId}")]
    public async Task<IActionResult> DeleteProperty(int propertyId)
    {
        var apiScopePropertiesDto = new ApiScopePropertiesDto { ApiScopePropertyId = propertyId };

        await _apiScopeService.GetApiScopePropertyAsync(apiScopePropertiesDto.ApiScopePropertyId);
        await _apiScopeService.DeleteApiScopePropertyAsync(apiScopePropertiesDto);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> PutScope([FromBody] ApiScopeViewModel apiScopeApi)
    {
        var apiScope = apiScopeApi.ToApiScopeViewModel<ApiScopeDto>();

        await _apiScopeService.GetApiScopeAsync(apiScope.Id);

        await _apiScopeService.UpdateApiScopeAsync(apiScope);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteScope(int id)
    {
        var apiScope = new ApiScopeDto { Id = id };

        await _apiScopeService.GetApiScopeAsync(apiScope.Id);

        await _apiScopeService.DeleteApiScopeAsync(apiScope);

        return Ok();
    }
}