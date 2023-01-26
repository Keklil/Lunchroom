using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Cart;

[Authorize]
public class Empty : PageModel
{
    public void OnGet()
    {
        
    }
}