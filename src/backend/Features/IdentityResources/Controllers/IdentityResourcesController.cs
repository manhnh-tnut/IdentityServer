using IdentityServer.Features.IdentityResources.Mappers;
using IdentityServer.Features.IdentityResources.Models;
using IdentityServer.Features.IdentityResources.Services.Interfaces;
using IdentityServer.Features.Shared.Resources.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Features.IdentityResources.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(Infrastructure.Filters.ControllerExceptionFilterAttribute))]
[Produces("application/json", "application/problem+json")]
[Authorize(Policy = Infrastructure.Constants.AuthorizationConstants.AdministrationPolicy)]
public class IdentityResourcesController : ControllerBase
{
    private readonly IIdentityResourceService _identityResourceService;
    private readonly IApiErrorResources _errorResources;

    public IdentityResourcesController(IIdentityResourceService identityResourceService, IApiErrorResources errorResources)
    {
        _identityResourceService = identityResourceService;
        _errorResources = errorResources;
    }

    [HttpGet]
    public async Task<ActionResult<IdentityResourcesViewModel>> Get(string searchText, int page = 1, int pageSize = 10)
    {
        var identityResourcesDto = await _identityResourceService.GetIdentityResourcesAsync(searchText, page, pageSize);
        var identityResourcesViewModel = identityResourcesDto.ToIdentityResourceViewModel<IdentityResourcesViewModel>();

        return Ok(identityResourcesViewModel);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IdentityResourceViewModel>> Get(int id)
    {
        var identityResourceDto = await _identityResourceService.GetIdentityResourceAsync(id);
        var identityResourceViewModel = identityResourceDto.ToIdentityResourceViewModel<IdentityResourceViewModel>();

        return Ok(identityResourceViewModel);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] IdentityResourceViewModel identityResourceApi)
    {
        var identityResourceDto = identityResourceApi.ToIdentityResourceViewModel<IdentityResourceDto>();

        if (!identityResourceDto.Id.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var id = await _identityResourceService.AddIdentityResourceAsync(identityResourceDto);
        identityResourceApi.Id = id;

        return CreatedAtAction(nameof(Get), new { id }, identityResourceApi);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] IdentityResourceViewModel identityResourceApi)
    {
        var identityResource = identityResourceApi.ToIdentityResourceViewModel<IdentityResourceDto>();

        await _identityResourceService.GetIdentityResourceAsync(identityResource.Id);
        await _identityResourceService.UpdateIdentityResourceAsync(identityResource);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var identityResource = new IdentityResourceDto { Id = id };

        await _identityResourceService.GetIdentityResourceAsync(identityResource.Id);
        await _identityResourceService.DeleteIdentityResourceAsync(identityResource);

        return Ok();
    }

    [HttpGet("{id}/Properties")]
    public async Task<ActionResult<IdentityResourcePropertiesViewModel>> GetProperties(int id, int page = 1, int pageSize = 10)
    {
        var identityResourcePropertiesDto = await _identityResourceService.GetIdentityResourcePropertiesAsync(id, page, pageSize);
        var identityResourcePropertiesViewModel = identityResourcePropertiesDto.ToIdentityResourceViewModel<IdentityResourcePropertiesViewModel>();

        return Ok(identityResourcePropertiesViewModel);
    }

    [HttpGet("Properties/{propertyId}")]
    public async Task<ActionResult<IdentityResourcePropertyViewModel>> GetProperty(int propertyId)
    {
        var identityResourcePropertiesDto = await _identityResourceService.GetIdentityResourcePropertyAsync(propertyId);
        var identityResourcePropertyViewModel = identityResourcePropertiesDto.ToIdentityResourceViewModel<IdentityResourcePropertyViewModel>();

        return Ok(identityResourcePropertyViewModel);
    }

    [HttpPost("{id}/Properties")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostProperty(int id, [FromBody] IdentityResourcePropertyViewModel identityResourcePropertyApi)
    {
        var identityResourcePropertiesDto = identityResourcePropertyApi.ToIdentityResourceViewModel<IdentityResourcePropertiesDto>();
        identityResourcePropertiesDto.IdentityResourceId = id;

        if (!identityResourcePropertiesDto.IdentityResourcePropertyId.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var propertyId = await _identityResourceService.AddIdentityResourcePropertyAsync(identityResourcePropertiesDto);
        identityResourcePropertyApi.Id = propertyId;

        return CreatedAtAction(nameof(GetProperty), new { propertyId }, identityResourcePropertyApi);
    }

    [HttpDelete("Properties/{propertyId}")]
    public async Task<IActionResult> DeleteProperty(int propertyId)
    {
        var identityResourceProperty = new IdentityResourcePropertiesDto { IdentityResourcePropertyId = propertyId };

        await _identityResourceService.GetIdentityResourcePropertyAsync(identityResourceProperty.IdentityResourcePropertyId);
        await _identityResourceService.DeleteIdentityResourcePropertyAsync(identityResourceProperty);

        return Ok();
    }
}