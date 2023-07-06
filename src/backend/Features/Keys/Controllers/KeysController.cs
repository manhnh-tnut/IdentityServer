using IdentityServer.Features.Keys.Mappers;
using IdentityServer.Features.Keys.Models;
using IdentityServer.Features.Keys.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Features.Keys.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(Infrastructure.Filters.ControllerExceptionFilterAttribute))]
[Produces("application/json")]
[Authorize(Policy = Infrastructure.Constants.AuthorizationConstants.AdministrationPolicy)]
public class KeysController : ControllerBase
{
    private readonly IKeyService _keyService;

    public KeysController(IKeyService keyService)
    {
        _keyService = keyService;
    }

    [HttpGet]
    public async Task<ActionResult<KeysViewModel>> Get(int page = 1, int pageSize = 10)
    {
        var keys = await _keyService.GetKeysAsync(page, pageSize);
        var keysApi = keys.ToKeyViewModel<KeysViewModel>();

        return Ok(keysApi);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<KeyViewModel>> Get(string id)
    {
        var key = await _keyService.GetKeyAsync(id);

        var keyApi = key.ToKeyViewModel<KeyViewModel>();

        return Ok(keyApi);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _keyService.DeleteKeyAsync(id);

        return Ok();
    }
}