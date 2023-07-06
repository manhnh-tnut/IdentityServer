using IdentityServer.Features.PersistedGrants.Mappers;
using IdentityServer.Features.PersistedGrants.Models;
using IdentityServer.Features.PersistedGrants.Services.Interfaces;
using IdentityServer.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Features.PersistedGrants.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(Infrastructure.Filters.ControllerExceptionFilterAttribute))]
[Produces("application/json")]
[Authorize(Policy = Infrastructure.Constants.AuthorizationConstants.AdministrationPolicy)]
public class PersistedGrantsController : ControllerBase
{
    private readonly IPersistedGrantService _persistedGrantsService;

    public PersistedGrantsController(IPersistedGrantService persistedGrantsService)
    {
        _persistedGrantsService = persistedGrantsService;
    }

    [HttpGet("Subjects")]
    public async Task<ActionResult<PersistedGrantSubjectsViewModel>> Get(string searchText, int page = 1, int pageSize = 10)
    {
        var persistedGrantsDto = await _persistedGrantsService.GetPersistedGrantsByUsersAsync(searchText, page, pageSize);
        var persistedGrantSubjectsViewModel = persistedGrantsDto.ToPersistedGrantViewModel<PersistedGrantSubjectsViewModel>();

        return Ok(persistedGrantSubjectsViewModel);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersistedGrantViewModel>> Get(string id)
    {
        var persistedGrantDto = await _persistedGrantsService.GetPersistedGrantAsync(UrlHelpers.QueryStringUnSafeHash(id));
        var persistedGrantViewModel = persistedGrantDto.ToPersistedGrantViewModel<PersistedGrantViewModel>();

        ParsePersistedGrantKey(persistedGrantViewModel);

        return Ok(persistedGrantViewModel);
    }

    [HttpGet("Subjects/{subjectId}")]
    public async Task<ActionResult<PersistedGrantsViewModel>> GetBySubject(string subjectId, int page = 1, int pageSize = 10)
    {
        var persistedGrantDto = await _persistedGrantsService.GetPersistedGrantsByUserAsync(subjectId, page, pageSize);
        var persistedGrantViewModel = persistedGrantDto.ToPersistedGrantViewModel<PersistedGrantsViewModel>();

        ParsePersistedGrantKeys(persistedGrantViewModel);

        return Ok(persistedGrantViewModel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _persistedGrantsService.DeletePersistedGrantAsync(UrlHelpers.QueryStringUnSafeHash(id));

        return Ok();
    }

    [HttpDelete("Subjects/{subjectId}")]
    public async Task<IActionResult> DeleteBySubject(string subjectId)
    {
        await _persistedGrantsService.DeletePersistedGrantsAsync(subjectId);

        return Ok();
    }

    private void ParsePersistedGrantKey(PersistedGrantViewModel persistedGrantViewModel)
    {
        if (!string.IsNullOrEmpty(persistedGrantViewModel.Key))
        {
            persistedGrantViewModel.Key = UrlHelpers.QueryStringSafeHash(persistedGrantViewModel.Key);
        }
    }

    private void ParsePersistedGrantKeys(PersistedGrantsViewModel persistedGrantViewModel)
    {
        persistedGrantViewModel.PersistedGrants.ForEach(ParsePersistedGrantKey);
    }
}