using IdentityServer.Features.ApiScopes.Models;
using IdentityServer.Features.ApiScopes.Services.Interfaces;
using IdentityServer.Infrastructure.Repositories.Interfaces;
using IdentityServer.Infrastructure.Exceptions;
using IdentityServer.Infrastructure.Common;
using Duende.IdentityServer.EntityFramework.Entities;
using IdentityServer.Infrastructure.Filters;
using IdentityServer.Features.ApiScopes.Mappers;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Features.ApiScopes.Resources.Interfaces;

namespace IdentityServer.Features.ApiScopes.Services;

public class ApiScopeService : IApiScopeService
{
    protected readonly IApiScopeRepository ApiScopeRepository;
    protected readonly IApiScopeServiceResources ApiScopeServiceResources;
    public ApiScopeService(IApiScopeServiceResources apiScopeServiceResources
    , IApiScopeRepository apiScopeRepository)
    {
        ApiScopeRepository = apiScopeRepository;
        ApiScopeServiceResources = apiScopeServiceResources;
    }

    public virtual async Task<ApiScopesDto> GetApiScopesAsync(string search, int page = 1, int pageSize = 10)
    {
        var pagedList = await ApiScopeRepository.GetApiScopesAsync(search, page, pageSize);

        var apiScopesDto = pagedList.ToModel();

        return apiScopesDto;
    }

    public virtual async Task<ICollection<string>> GetApiScopesNameAsync(string scope, int limit = 0)
    {
        var scopes = await ApiScopeRepository.GetApiScopesNameAsync(scope, limit);

        return scopes;
    }

    public virtual async Task<ApiScopePropertiesDto> GetApiScopePropertiesAsync(int apiScopeId, int page = 1, int pageSize = 10)
    {
        var apiScope = await ApiScopeRepository.GetApiScopeAsync(apiScopeId);
        if (apiScope == null)
            throw new UserFriendlyErrorPageException(string.Format(ApiScopeServiceResources.ApiScopeDoesNotExist().Description, apiScopeId), ApiScopeServiceResources.ApiScopeDoesNotExist().Description);

        PagedList<ApiScopeProperty> pagedList = await ApiScopeRepository.GetApiScopePropertiesAsync(apiScopeId, page, pageSize);
        var apiScopePropertiesDto = pagedList.ToModel();
        apiScopePropertiesDto.ApiScopeId = apiScopeId;
        apiScopePropertiesDto.ApiScopeName = await ApiScopeRepository.GetApiScopeNameAsync(apiScopeId);

        return apiScopePropertiesDto;
    }

    public virtual async Task<ApiScopeDto> GetApiScopeAsync(int apiScopeId)
    {
        var apiScope = await ApiScopeRepository.GetApiScopeAsync(apiScopeId);
        if (apiScope == null) throw new UserFriendlyErrorPageException(string.Format(ApiScopeServiceResources.ApiScopeDoesNotExist().Description, apiScopeId), ApiScopeServiceResources.ApiScopeDoesNotExist().Description);

        var apiScopeDto = apiScope.ToModel();

        return apiScopeDto;
    }

    public virtual async Task<int> AddApiScopeAsync(ApiScopeDto apiScope)
    {
        var canInsert = await CanInsertApiScopeAsync(apiScope);
        if (!canInsert)
        {
            throw new UserFriendlyViewException(string.Format(ApiScopeServiceResources.ApiScopeExistsValue().Description, apiScope.Name), ApiScopeServiceResources.ApiScopeExistsKey().Description, apiScope);
        }

        var scope = apiScope.ToEntity();

        var added = await ApiScopeRepository.AddApiScopeAsync(scope);

        return added;
    }

    public virtual ApiScopeDto BuildApiScopeViewModel(ApiScopeDto apiScope)
    {
        ComboBoxHelpers.PopulateValuesToList(apiScope.UserClaimsItems, apiScope.UserClaims);

        return apiScope;
    }

    public virtual async Task<int> UpdateApiScopeAsync(ApiScopeDto apiScope)
    {
        var canInsert = await CanInsertApiScopeAsync(apiScope);
        if (!canInsert)
        {
            throw new UserFriendlyViewException(string.Format(ApiScopeServiceResources.ApiScopeExistsValue().Description, apiScope.Name), ApiScopeServiceResources.ApiScopeExistsKey().Description, apiScope);
        }

        var scope = apiScope.ToEntity();

        var originalApiScope = await GetApiScopeAsync(apiScope.Id);

        var updated = await ApiScopeRepository.UpdateApiScopeAsync(scope);

        return updated;
    }

    public virtual async Task<int> DeleteApiScopeAsync(ApiScopeDto apiScope)
    {
        var scope = apiScope.ToEntity();

        var deleted = await ApiScopeRepository.DeleteApiScopeAsync(scope);

        return deleted;
    }

    public virtual async Task<bool> CanInsertApiScopeAsync(ApiScopeDto apiScopeDto)
    {
        var apiScope = apiScopeDto.ToEntity();

        return await ApiScopeRepository.CanInsertApiScopeAsync(apiScope);
    }

    public virtual async Task<ApiScopePropertiesDto> GetApiScopePropertyAsync(int apiScopePropertyId)
    {
        var apiScopeProperty = await ApiScopeRepository.GetApiScopePropertyAsync(apiScopePropertyId);
        if (apiScopeProperty == null) throw new UserFriendlyErrorPageException(string.Format(ApiScopeServiceResources.ApiScopePropertyDoesNotExist().Description, apiScopePropertyId));

        var apiScopePropertiesDto = apiScopeProperty.ToModel();
        apiScopePropertiesDto.ApiScopeId = apiScopeProperty.ScopeId;
        apiScopePropertiesDto.ApiScopeName = await ApiScopeRepository.GetApiScopeNameAsync(apiScopeProperty.ScopeId);

        return apiScopePropertiesDto;
    }

    private async Task BuildApiScopePropertiesViewModelAsync(ApiScopePropertiesDto apiScopeProperties)
    {
        var apiResourcePropertiesDto = await GetApiScopePropertiesAsync(apiScopeProperties.ApiScopeId);
        apiScopeProperties.ApiScopeProperties.AddRange(apiResourcePropertiesDto.ApiScopeProperties);
        apiScopeProperties.TotalCount = apiResourcePropertiesDto.TotalCount;
    }

    public virtual async Task<int> AddApiScopePropertyAsync(ApiScopePropertiesDto apiScopeProperties)
    {
        var canInsert = await CanInsertApiScopePropertyAsync(apiScopeProperties);
        if (!canInsert)
        {
            await BuildApiScopePropertiesViewModelAsync(apiScopeProperties);
            throw new UserFriendlyViewException(string.Format(ApiScopeServiceResources.ApiScopePropertyExistsValue().Description, apiScopeProperties.Key), ApiScopeServiceResources.ApiScopePropertyExistsKey().Description, apiScopeProperties);
        }

        var apiScopeProperty = apiScopeProperties.ToEntity();

        var saved = await ApiScopeRepository.AddApiScopePropertyAsync(apiScopeProperties.ApiScopeId, apiScopeProperty);

        return saved;
    }

    public virtual async Task<int> DeleteApiScopePropertyAsync(ApiScopePropertiesDto apiScopeProperty)
    {
        var propertyEntity = apiScopeProperty.ToEntity();

        var deleted = await ApiScopeRepository.DeleteApiScopePropertyAsync(propertyEntity);

        return deleted;
    }

    public virtual async Task<bool> CanInsertApiScopePropertyAsync(ApiScopePropertiesDto apiResourceProperty)
    {
        var resource = apiResourceProperty.ToEntity();

        return await ApiScopeRepository.CanInsertApiScopePropertyAsync(resource);
    }
}