using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Menu;

[Authorize]
public class TimeOut : PageModel
{
    public void OnGet()
    {
    }
}