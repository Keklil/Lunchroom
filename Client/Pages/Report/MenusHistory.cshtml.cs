using Client.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Report;

[Authorize(Roles = "admin")]
public class MenusHistory : PageModel
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IApiClient _api;
    private readonly ILogger<IndexModel> _logger;

    public List<MenuForList> Menus { get; set; }

    [BindProperty] public string MenuDate { get; set; }

    public async Task<ActionResult> OnGetAsync()
    {
        Menus = await _api.Menu_GetAllMenusAsync() as List<MenuForList>;

        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        var date = MenuDate;
        return Redirect("/Report/OrdersReport?date=" + date);
    }

    public MenusHistory(ILogger<IndexModel> logger, IApiClient api, IHttpContextAccessor accessor)
    {
        _logger = logger;
        _api = api;
        _accessor = accessor;
    }
}