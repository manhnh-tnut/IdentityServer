using IdentityServer.Features.Shared.Models;
using IdentityServer.Features.Shared.Resources.Interfaces;

namespace IdentityServer.Features.Shared.Resources;

public class ApiErrorResources : IApiErrorResources
{
    public virtual ApiError CannotSetId()
    {
        return new ApiError
        {
            Code = nameof(CannotSetId),
            Description = "Cannot set ID when creating a new entity"
        };
    }
}