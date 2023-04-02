using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Order;

public class MenuExpired : PageModel
{
    private readonly IConfiguration _configuration;
    public int hour, minute;

    public void OnGet()
    {
    }

    public MenuExpired(IConfiguration configuration)
    {
        _configuration = configuration;

        hour = _configuration.GetValue<int>("TimeMenuExpiration:Hour");
        minute = _configuration.GetValue<int>("TimeMenuExpiration:Minute");
    }
}