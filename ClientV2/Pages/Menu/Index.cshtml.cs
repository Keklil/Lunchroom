using System.ComponentModel.DataAnnotations;
using ClientV2.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Menu;

public class Index : PageModel
{
    private readonly IApiClientV2 _api;
    private readonly IHttpContextAccessor _accessor;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public List<Guid> CheckedOption { get; set; }
    [BindProperty]
    public string CheckedLunchSet { get; set; }
    public MenuDto? Menu { get; set; }
    
    public Index(IApiClientV2 api, IHttpContextAccessor accessor,
        IConfiguration configuration)
    {
        _api = api;
        _accessor = accessor;
        _configuration = configuration;
    }
    
    public async Task<ActionResult> OnGetAsync()
    {
        var hour = 23;
        var minute = 59;
        
        var group = await _api.Group_GetGroupAsync(new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        if (group.Settings is not null)
        {
            hour = group.Settings.HourExpired;
            minute = group.Settings.MinuteExpired;
        }

        if (DateTime.Now.TimeOfDay > new TimeSpan(hour, minute, 0))
            return RedirectToPage("/Menu/TimeOut");
        
        Menu = await _api.Menu_GetTodayMenuAsync(new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        if (Menu is null)
        {
            return RedirectToPage("/Menu/NotReady");
        }

        Menu.LunchSets = Menu.LunchSets.OrderBy(x => x.Price).ToList();
        Menu.Options = Menu.Options.OrderBy(x => x.Price).ToList();
        
        HttpContext.Session.SetString("MenuId", Menu.Id.ToString());
        return Page();
    }
}