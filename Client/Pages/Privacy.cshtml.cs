using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public void OnGet()
    {
    }

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }
}