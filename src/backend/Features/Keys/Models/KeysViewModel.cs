namespace IdentityServer.Features.Keys.Models;

public class KeysViewModel
{
    public KeysViewModel()
    {
        Keys = new List<KeyViewModel>();
    }

    public List<KeyViewModel> Keys { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}