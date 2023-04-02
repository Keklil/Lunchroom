using System.Text.Json;
using ClientV2.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Menu;

[Authorize]
public class Index : PageModel
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IApiClientV2 _api;
    private readonly IConfiguration _configuration;

    [BindProperty] public List<Guid> CheckedOption { get; set; }

    [BindProperty] public int LunchSetUnits { get; set; }

    [BindProperty] public string CheckedLunchSet { get; set; }

    public MenuDto? Menu { get; set; }

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
        if (Menu is null) return RedirectToPage("/Menu/NotReady");

        Menu.LunchSets = Menu.LunchSets.OrderBy(x => x.Price).ToList();
        Menu.Options = Menu.Options.OrderBy(x => x.Price).ToList();

        HttpContext.Session.SetString("MenuId", Menu.Id.ToString());
        return Page();
    }


    public async Task<ActionResult> OnPostAsync()
    {
        var menuIdRaw = HttpContext.Session.GetString("MenuId");
        Guid.TryParse(menuIdRaw, out var menuId);

        var container = new OrderContainer
        {
            Id = Guid.NewGuid(),
            OptionsIds = new List<Guid>(),
            MenuId = menuId
        };

        if (LunchSetUnits > 0 &&
            !string.IsNullOrWhiteSpace(CheckedLunchSet) &&
            Guid.TryParse(CheckedLunchSet, out var lunchId))
        {
            container.LunchSetId = lunchId;
            container.LunchSetUnits = LunchSetUnits;
        }

        if (CheckedOption is not null && CheckedOption.Count > 0) container.OptionsIds = CheckedOption;

        if (container.LunchSetUnits > 0 || container.OptionsIds.Count > 0)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (!string.IsNullOrWhiteSpace(cart))
            {
                var cartEntity = JsonSerializer.Deserialize<List<OrderContainer>>(cart);
                cartEntity.Add(container);
                var cartJson = JsonSerializer.Serialize(cartEntity);
                HttpContext.Session.SetString("cart", cartJson);
            }
            else
            {
                var newCart = new List<OrderContainer>();
                newCart.Add(container);
                var cartJson = JsonSerializer.Serialize(newCart);
                HttpContext.Session.SetString("cart", cartJson);
            }
        }

        return Redirect("/Menu");
    }

    public Index(IApiClientV2 api, IHttpContextAccessor accessor,
        IConfiguration configuration)
    {
        _api = api;
        _accessor = accessor;
        _configuration = configuration;
    }
}

public class OrderContainer
{
    public Guid MenuId { get; set; }
    public Guid Id { get; set; }
    public Guid LunchSetId { get; set; }
    public int LunchSetUnits { get; set; }
    public List<Guid> OptionsIds { get; set; }
}