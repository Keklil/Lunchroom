using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Account;

public class EmailConfirmation : PageModel
{
    public bool Success { get; set; } = true;
    public void OnGet()
    {
        
    }

    public void OnGetError()
    {
        Success = false;
    }
}