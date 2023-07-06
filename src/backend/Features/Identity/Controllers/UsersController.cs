using AutoMapper;
using IdentityModel;
using IdentityServer.Features.Identity.Controllers.Interfaces;
using IdentityServer.Features.Identity.Models;
using IdentityServer.Features.Identity.Services.Interfaces;
using IdentityServer.Features.Shared.Resources.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Features.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(Infrastructure.Filters.ControllerExceptionFilterAttribute))]
[Produces("application/json", "application/problem+json")]
[Authorize(Policy = Infrastructure.Constants.AuthorizationConstants.AdministrationPolicy)]
public class UsersController<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto> : ControllerBase
        where TUserDto : UserDto<TKey>, new()
        where TRoleDto : RoleDto<TKey>, new()
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TUsersDto : UsersDto<TUserDto, TKey>
        where TRolesDto : RolesDto<TRoleDto, TKey>
        where TUserRolesDto : UserRolesDto<TRoleDto, TKey>
        where TUserClaimsDto : UserClaimsDto<TUserClaimDto, TKey>, new()
        where TUserProviderDto : UserProviderDto<TKey>
        where TUserProvidersDto : UserProvidersDto<TUserProviderDto, TKey>
        where TUserChangePasswordDto : UserChangePasswordDto<TKey>
        where TRoleClaimsDto : RoleClaimsDto<TRoleClaimDto, TKey>
        where TUserClaimDto : UserClaimDto<TKey>
        where TRoleClaimDto : RoleClaimDto<TKey>
{
    private readonly IIdentityService<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto> _identityService;
    private readonly IGenericControllerLocalizer<UsersController<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>> _localizer;

    private readonly IMapper _mapper;
    private readonly IApiErrorResources _errorResources;

    public UsersController(IIdentityService<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto> identityService,
        IGenericControllerLocalizer<UsersController<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>> localizer, IMapper mapper, IApiErrorResources errorResources)
    {
        _identityService = identityService;
        _localizer = localizer;
        _mapper = mapper;
        _errorResources = errorResources;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TUserDto>> Get(TKey id)
    {
        var user = await _identityService.GetUserAsync(id.ToString());

        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<TUsersDto>> Get(string searchText, int page = 1, int pageSize = 10)
    {
        var usersDto = await _identityService.GetUsersAsync(searchText, page, pageSize);

        return Ok(usersDto);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TUserDto>> Post([FromBody] TUserDto user)
    {
        if (!EqualityComparer<TKey>.Default.Equals(user.Id, default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        var (identityResult, userId) = await _identityService.CreateUserAsync(user);
        var createdUser = await _identityService.GetUserAsync(userId.ToString());

        return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] TUserDto user)
    {
        await _identityService.GetUserAsync(user.Id.ToString());
        await _identityService.UpdateUserAsync(user);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(TKey id)
    {
        if (IsDeleteForbidden(id))
            return StatusCode((int)System.Net.HttpStatusCode.Forbidden);

        var user = new TUserDto { Id = id };

        await _identityService.GetUserAsync(user.Id.ToString());
        await _identityService.DeleteUserAsync(user.Id.ToString(), user);

        return Ok();
    }

    private bool IsDeleteForbidden(TKey id)
    {
        var userId = User.FindFirst(JwtClaimTypes.Subject);

        return userId == null ? false : userId.Value == id.ToString();
    }

    [HttpGet("{id}/Roles")]
    public async Task<ActionResult<UserRolesViewModel<TRoleDto>>> GetUserRoles(TKey id, int page = 1, int pageSize = 10)
    {
        var userRoles = await _identityService.GetUserRolesAsync(id.ToString(), page, pageSize);
        var userRolesViewModel = _mapper.Map<UserRolesViewModel<TRoleDto>>(userRoles);

        return Ok(userRolesViewModel);
    }

    [HttpPost("Roles")]
    public async Task<IActionResult> PostUserRoles([FromBody] UserRoleViewModel<TKey> role)
    {
        var userRolesDto = _mapper.Map<TUserRolesDto>(role);

        await _identityService.GetUserAsync(userRolesDto.UserId.ToString());
        await _identityService.GetRoleAsync(userRolesDto.RoleId.ToString());

        await _identityService.CreateUserRoleAsync(userRolesDto);

        return Ok();
    }

    [HttpDelete("Roles")]
    public async Task<IActionResult> DeleteUserRoles([FromBody] UserRoleViewModel<TKey> role)
    {
        var userRolesDto = _mapper.Map<TUserRolesDto>(role);

        await _identityService.GetUserAsync(userRolesDto.UserId.ToString());
        await _identityService.GetRoleAsync(userRolesDto.RoleId.ToString());

        await _identityService.DeleteUserRoleAsync(userRolesDto);

        return Ok();
    }

    [HttpGet("{id}/Claims")]
    public async Task<ActionResult<UserClaimsViewModel<TKey>>> GetUserClaims(TKey id, int page = 1, int pageSize = 10)
    {
        var claims = await _identityService.GetUserClaimsAsync(id.ToString(), page, pageSize);

        var userClaimsViewModel = _mapper.Map<UserClaimsViewModel<TKey>>(claims);

        return Ok(userClaimsViewModel);
    }

    [HttpPost("Claims")]
    public async Task<IActionResult> PostUserClaims([FromBody] UserClaimViewModel<TKey> claim)
    {
        var userClaimDto = _mapper.Map<TUserClaimsDto>(claim);

        if (!userClaimDto.ClaimId.Equals(default))
        {
            return BadRequest(_errorResources.CannotSetId());
        }

        await _identityService.CreateUserClaimsAsync(userClaimDto);

        return Ok();
    }

    [HttpPut("Claims")]
    public async Task<IActionResult> PutUserClaims([FromBody] UserClaimViewModel<TKey> claim)
    {
        var userClaimDto = _mapper.Map<TUserClaimsDto>(claim);

        await _identityService.GetUserClaimAsync(userClaimDto.UserId.ToString(), userClaimDto.ClaimId);
        await _identityService.UpdateUserClaimsAsync(userClaimDto);

        return Ok();
    }

    [HttpDelete("{id}/Claims")]
    public async Task<IActionResult> DeleteUserClaims([FromRoute] TKey id, int claimId)
    {
        var userClaimsDto = new TUserClaimsDto
        {
            ClaimId = claimId,
            UserId = id
        };

        await _identityService.GetUserClaimAsync(id.ToString(), claimId);
        await _identityService.DeleteUserClaimAsync(userClaimsDto);

        return Ok();
    }

    [HttpGet("{id}/Providers")]
    public async Task<ActionResult<UserProvidersViewModel<TKey>>> GetUserProviders(TKey id)
    {
        var userProvidersDto = await _identityService.GetUserProvidersAsync(id.ToString());
        var userProvidersViewModel = _mapper.Map<UserProvidersViewModel<TKey>>(userProvidersDto);

        return Ok(userProvidersViewModel);
    }

    [HttpDelete("Providers")]
    public async Task<IActionResult> DeleteUserProviders([FromBody] UserProviderDeleteViewModel<TKey> provider)
    {
        var providerDto = _mapper.Map<TUserProviderDto>(provider);

        await _identityService.GetUserProviderAsync(providerDto.UserId.ToString(), providerDto.ProviderKey);
        await _identityService.DeleteUserProvidersAsync(providerDto);

        return Ok();
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> PostChangePassword([FromBody] UserChangePasswordViewModel<TKey> password)
    {
        var userChangePasswordDto = _mapper.Map<TUserChangePasswordDto>(password);

        await _identityService.UserChangePasswordAsync(userChangePasswordDto);

        return Ok();
    }

    [HttpGet("{id}/RoleClaims")]
    public async Task<ActionResult<RoleClaimsViewModel<TKey>>> GetRoleClaims(TKey id, string claimSearchText, int page = 1, int pageSize = 10)
    {
        var roleClaimsDto = await _identityService.GetUserRoleClaimsAsync(id.ToString(), claimSearchText, page, pageSize);
        var roleClaimsViewModel = _mapper.Map<RoleClaimsViewModel<TKey>>(roleClaimsDto);

        return Ok(roleClaimsViewModel);
    }

    [HttpGet("ClaimType/{claimType}/ClaimValue/{claimValue}")]
    public async Task<ActionResult<TUsersDto>> GetClaimUsers(string claimType, string claimValue, int page = 1, int pageSize = 10)
    {
        var usersDto = await _identityService.GetClaimUsersAsync(claimType, claimValue, page, pageSize);

        return Ok(usersDto);
    }

    [HttpGet("ClaimType/{claimType}")]
    public async Task<ActionResult<TUsersDto>> GetClaimUsers(string claimType, int page = 1, int pageSize = 10)
    {
        var usersDto = await _identityService.GetClaimUsersAsync(claimType, null, page, pageSize);

        return Ok(usersDto);
    }
}