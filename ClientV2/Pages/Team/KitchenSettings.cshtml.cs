using ClientV2.Apis;
using ClientV2.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientV2.Pages.Team;

public class KitchenSettings : PageModel
{
    private readonly IApiClientV2 _api;

    public KitchenSettings(IApiClientV2 api)
    {
        _api = api;
    }
    
    [BindProperty]
    public Guid GroupId { get; set; }
    
    [BindProperty]
    public string Name { get; set; }
    
    [BindProperty]
    public string Email { get; set; }
    
    [BindProperty]
    public TimeOnly MenuExpired { get; set; }
    
    [BindProperty]
    public ClientV2.Apis.PeriodicRefresh PeriodicRefresh { get; set; }
    public SelectList PeriodicRefreshList => PeriodicRefresh.ToEnumDescriptionsList();
    
    [BindProperty]
    public ClientV2.Apis.MenuFormat MenuFormat { get; set; }
    public SelectList MenuFormatList => MenuFormat.ToEnumDescriptionsList();
    
    public async Task<ActionResult> OnGetAsync()
    {
        var groupSettings = await _api.Group_GetGroupAsync(new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        GroupId = groupSettings.Id;
        
        if (groupSettings.Settings is null) return Page();


        Name = groupSettings.Settings.KitchenName;
        Email = groupSettings.Settings.TargetEmail;
        MenuExpired = new TimeOnly(groupSettings.Settings.HourExpired, groupSettings.Settings.MinuteExpired);
        PeriodicRefresh = groupSettings.Settings.PeriodicRefresh;
        MenuFormat = groupSettings.Settings.MenuFormat;
        
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        var settings = new GroupConfigDto()
        {
            GroupId = this.GroupId,
            HourExpired = MenuExpired.Hour,
            MinuteExpired = MenuExpired.Minute,
            MenuFormat = MenuFormat,
            KitchenName = Name,
            Refresh = PeriodicRefresh,
            TargetEmail = Email
        };

        _api.Group_ConfigureKitchenAsync(settings);

        return RedirectToPage(nameof(Menu.Index));
    }
}
