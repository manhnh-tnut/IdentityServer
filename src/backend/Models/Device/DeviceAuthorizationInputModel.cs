using IdentityServer.Models.Consent;

namespace IdentityServer.Models.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string UserCode { get; set; }
}