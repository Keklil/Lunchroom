using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Order;

[Authorize]
public class TodayMenuNotFound : PageModel
{
    public void OnGet()
    {
    }
}