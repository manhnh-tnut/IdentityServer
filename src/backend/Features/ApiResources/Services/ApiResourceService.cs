using Duende.IdentityServer.Models;
using IdentityServer.Features.ApiResources.Services.Interfaces;
using IdentityServer.Features.Clients.Services.Interfaces;
using IdentityServer.Features.ApiResources.Mappers;
using IdentityServer.Features.ApiResources.Models;
using IdentityServer.Infrastructure.Enums;
using IdentityServer.Infrastructure.Exceptions;
using IdentityServer.Infrastructure.Filters;
using IdentityServer.Infrastructure.Repositories.Interfaces;
using IdentityServer.Features.ApiResources.Resources;

namespace IdentityServer.Features.ApiResources.Services;

public class ApiResourceService : IApiResourceService
{
    protected readonly IApiResourceRepository _apiResourceRepository;
    protected readonly ApiResourceServiceResources _apiResourcesService;
    protected readonly IClientService _clientService;
    private const string SharedSecret = "SharedSecret";

    public ApiResourceService(IApiResourceRepository apiResourceRepository,
        ApiResourceServiceResources apiResourcesService,
        IClientService clientService)
    {
        _apiResourceRepository = apiResourceRepository;
        _apiResourcesService = apiResourcesService;
        _clientService = clientService;
    }

    public virtual async Task<ApiResourcesDto> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10)
    {
        var pagedList = await _apiResourceRepository.GetApiResourcesAsync(search, page, pageSize);
        var apiResourcesDto = pagedList.ToModel();

        return apiResourcesDto;
    }

    public virtual async Task<ApiResourcePropertiesDto> GetApiResourcePropertiesAsync(int apiResourceId, int page = 1, int pageSize = 10)
    {
        var apiResource = await _apiResourceRepository.GetApiResourceAsync(apiResourceId);
        if (apiResource == null) throw new UserFriendlyErrorPageException(string.Format(_apiResourcesService.ApiResourceDoesNotExist().Description, apiResourceId), _apiResourcesService.ApiResourceDoesNotExist().Description);

        var pagedList = await _apiResourceRepository.GetApiResourcePropertiesAsync(apiResourceId, page, pageSize);
        var apiResourcePropertiesDto = pagedList.ToModel();
        apiResourcePropertiesDto.ApiResourceId = apiResourceId;
        apiResourcePropertiesDto.ApiResourceName = await _apiResourceRepository.GetApiResourceNameAsync(apiResourceId);

        return apiResourcePropertiesDto;
    }

    public virtual async Task<ApiResourcePropertiesDto> GetApiResourcePropertyAsync(int apiResourcePropertyId)
    {
        var apiResourceProperty = await _apiResourceRepository.GetApiResourcePropertyAsync(apiResourcePropertyId);
        if (apiResourceProperty == null) throw new UserFriendlyErrorPageException(string.Format(_apiResourcesService.ApiResourcePropertyDoesNotExist().Description, apiResourcePropertyId));

        var apiResourcePropertiesDto = apiResourceProperty.ToModel();
        apiResourcePropertiesDto.ApiResourceId = apiResourceProperty.ApiResourceId;
        apiResourcePropertiesDto.ApiResourceName = await _apiResourceRepository.GetApiResourceNameAsync(apiResourceProperty.ApiResourceId);

        return apiResourcePropertiesDto;
    }

    public virtual async Task<int> AddApiResourcePropertyAsync(ApiResourcePropertiesDto apiResourceProperties)
    {
        var canInsert = await CanInsertApiResourcePropertyAsync(apiResourceProperties);
        if (!canInsert)
        {
            await BuildApiResourcePropertiesViewModelAsync(apiResourceProperties);
            throw new UserFriendlyViewException(string.Format(_apiResourcesService.ApiResourcePropertyExistsValue().Description, apiResourceProperties.Key), _apiResourcesService.ApiResourcePropertyExistsKey().Description, apiResourceProperties);
        }

        var apiResourceProperty = apiResourceProperties.ToEntity();
        var saved = await _apiResourceRepository.AddApiResourcePropertyAsync(apiResourceProperties.ApiResourceId, apiResourceProperty);

        return saved;
    }

    public virtual async Task<int> DeleteApiResourcePropertyAsync(ApiResourcePropertiesDto apiResourceProperty)
    {
        var propertyEntity = apiResourceProperty.ToEntity();
        var deleted = await _apiResourceRepository.DeleteApiResourcePropertyAsync(propertyEntity);

        return deleted;
    }

    public virtual async Task<bool> CanInsertApiResourcePropertyAsync(ApiResourcePropertiesDto apiResourceProperty)
    {
        var resource = apiResourceProperty.ToEntity();

        return await _apiResourceRepository.CanInsertApiResourcePropertyAsync(resource);
    }

    private void HashApiSharedSecret(ApiSecretsDto apiSecret)
    {
        if (apiSecret.Type != SharedSecret) return;

        if (apiSecret.HashTypeEnum == HashType.Sha256)
        {
            apiSecret.Value = apiSecret.Value.Sha256();
        }
        else if (apiSecret.HashTypeEnum == HashType.Sha512)
        {
            apiSecret.Value = apiSecret.Value.Sha512();
        }
    }

    public virtual ApiSecretsDto BuildApiSecretsViewModel(ApiSecretsDto apiSecrets)
    {
        apiSecrets.HashTypes = _clientService.GetHashTypes();
        apiSecrets.TypeList = _clientService.GetSecretTypes();

        return apiSecrets;
    }

    public virtual async Task<ApiResourceDto> GetApiResourceAsync(int apiResourceId)
    {
        var apiResource = await _apiResourceRepository.GetApiResourceAsync(apiResourceId);
        if (apiResource == null) throw new UserFriendlyErrorPageException(_apiResourcesService.ApiResourceDoesNotExist().Description, _apiResourcesService.ApiResourceDoesNotExist().Description);
        var apiResourceDto = apiResource.ToModel();

        return apiResourceDto;
    }

    public virtual async Task<int> AddApiResourceAsync(ApiResourceDto apiResource)
    {
        var canInsert = await CanInsertApiResourceAsync(apiResource);
        if (!canInsert)
        {
            throw new UserFriendlyViewException(string.Format(_apiResourcesService.ApiResourceExistsValue().Description, apiResource.Name), _apiResourcesService.ApiResourceExistsKey().Description, apiResource);
        }

        var resource = apiResource.ToEntity();

        var added = await _apiResourceRepository.AddApiResourceAsync(resource);

        return added;
    }

    public virtual async Task<int> UpdateApiResourceAsync(ApiResourceDto apiResource)
    {
        var canInsert = await CanInsertApiResourceAsync(apiResource);
        if (!canInsert)
        {
            throw new UserFriendlyViewException(string.Format(_apiResourcesService.ApiResourceExistsValue().Description, apiResource.Name), _apiResourcesService.ApiResourceExistsKey().Description, apiResource);
        }

        var resource = apiResource.ToEntity();

        var originalApiResource = await GetApiResourceAsync(apiResource.Id);

        var updated = await _apiResourceRepository.UpdateApiResourceAsync(resource);

        return updated;
    }

    public virtual async Task<int> DeleteApiResourceAsync(ApiResourceDto apiResource)
    {
        var resource = apiResource.ToEntity();

        var deleted = await _apiResourceRepository.DeleteApiResourceAsync(resource);

        return deleted;
    }

    public virtual async Task<bool> CanInsertApiResourceAsync(ApiResourceDto apiResource)
    {
        var resource = apiResource.ToEntity();

        return await _apiResourceRepository.CanInsertApiResourceAsync(resource);
    }

    private async Task BuildApiResourcePropertiesViewModelAsync(ApiResourcePropertiesDto apiResourceProperties)
    {
        var apiResourcePropertiesDto = await GetApiResourcePropertiesAsync(apiResourceProperties.ApiResourceId);
        apiResourceProperties.ApiResourceProperties.AddRange(apiResourcePropertiesDto.ApiResourceProperties);
        apiResourceProperties.TotalCount = apiResourcePropertiesDto.TotalCount;
    }

    public virtual async Task<ApiSecretsDto> GetApiSecretsAsync(int apiResourceId, int page = 1, int pageSize = 10)
    {
        var apiResource = await _apiResourceRepository.GetApiResourceAsync(apiResourceId);
        if (apiResource == null) throw new UserFriendlyErrorPageException(string.Format(_apiResourcesService.ApiResourceDoesNotExist().Description, apiResourceId), _apiResourcesService.ApiResourceDoesNotExist().Description);

        var pagedList = await _apiResourceRepository.GetApiSecretsAsync(apiResourceId, page, pageSize);

        var apiSecretsDto = pagedList.ToModel();
        apiSecretsDto.ApiResourceId = apiResourceId;
        apiSecretsDto.ApiResourceName = await _apiResourceRepository.GetApiResourceNameAsync(apiResourceId);

        // remove secret value from dto
        apiSecretsDto.ApiSecrets.ForEach(x => x.Value = null);

        return apiSecretsDto;
    }

    public virtual async Task<int> AddApiSecretAsync(ApiSecretsDto apiSecret)
    {
        HashApiSharedSecret(apiSecret);

        var secret = apiSecret.ToEntity();

        var added = await _apiResourceRepository.AddApiSecretAsync(apiSecret.ApiResourceId, secret);

        return added;
    }

    public virtual async Task<ApiSecretsDto> GetApiSecretAsync(int apiSecretId)
    {
        var apiSecret = await _apiResourceRepository.GetApiSecretAsync(apiSecretId);
        if (apiSecret == null) throw new UserFriendlyErrorPageException(string.Format(_apiResourcesService.ApiSecretDoesNotExist().Description, apiSecretId), _apiResourcesService.ApiSecretDoesNotExist().Description);
        var apiSecretsDto = apiSecret.ToModel();

        // remove secret value for dto
        apiSecretsDto.Value = null;

        return apiSecretsDto;
    }

    public virtual async Task<int> DeleteApiSecretAsync(ApiSecretsDto apiSecret)
    {
        var secret = apiSecret.ToEntity();
        var deleted = await _apiResourceRepository.DeleteApiSecretAsync(secret);

        return deleted;
    }

    public virtual async Task<string> GetApiResourceNameAsync(int apiResourceId)
    {
        return await _apiResourceRepository.GetApiResourceNameAsync(apiResourceId);
    }
}