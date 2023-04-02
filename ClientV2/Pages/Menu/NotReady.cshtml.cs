using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Menu;

[Authorize]
public class NotReady : PageModel
{
    public void OnGet()
    {
    }
}