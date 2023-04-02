using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages;

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