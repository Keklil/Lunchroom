using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using Client.Apis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Order;

[Authorize]
public class SelectItemsForOrder : PageModel
{
    private readonly IApiClient _api;
    private readonly IHttpContextAccessor _accessor;
    private readonly IConfiguration _configuration;
    private int hour, minute;
    
    [BindProperty]
    public List<Guid> CheckedOption { get; set; }
    [Required(ErrorMessage = "Пожалуйста, выберите обеденный набор"), BindProperty]
    public string CheckedLunchSet { get; set; }
    public MenuDto? Menu { get; set; }

    
    public SelectItemsForOrder(IApiClient api, IHttpContextAccessor accessor,
        IConfiguration configuration)
    {
        _api = api;
        _accessor = accessor;
        _configuration = configuration;

        hour = _configuration.GetValue<int>("TimeMenuExpiration:Hour");
        minute = _configuration.GetValue<int>("TimeMenuExpiration:Minute");
    }

    public async Task<ActionResult> OnGetAsync()
    {
        if (DateTime.Now.TimeOfDay > new TimeSpan(hour, minute, 0))
            return RedirectToPage("/Order/MenuExpired");
        
        Menu = await _api.Menu_GetTodayMenuAsync();
        if (Menu is null)
        {
            return RedirectToPage("/Order/TodayMenuNotFound");
        }

        Menu.LunchSets = Menu.LunchSets.OrderBy(x => x.Price).ToList();
        Menu.Options = Menu.Options.OrderBy(x => x.Price).ToList();
        
        HttpContext.Session.SetString("MenuId", Menu.Id.ToString());
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (DateTime.Now.TimeOfDay > new TimeSpan(hour, minute, 0))
            return RedirectToPage("/Order/MenuExpired");
        
        if (ModelState.IsValid)
        {
           var menuId = Guid.Parse(HttpContext.Session.GetString("MenuId"));
           var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
           var userId = Guid.Parse(userIdFromContext);
           
           var userInfo = await _api.User_GetUserAsync(userId);
           if (userInfo.Name is null || userInfo.Name.Length < 1 ||
               userInfo.Surname is null || userInfo.Surname.Length < 1)
               return RedirectToPage("/Account/Edit");
                      
           Menu = await _api.Menu_GetTodayMenuAsync();

           decimal optionsPrice = 0m;
           
           var options = new Collection<OrderOptionForCreationDto>();
           foreach (var id in CheckedOption)
           {
               options.Add(new OrderOptionForCreationDto(){OptionId = id, Units = 1});
               var optionPrice = Menu.Options.Where(x => x.Id == id)
                   .Single()
                   .Price;
               optionsPrice += optionPrice;
           }

           var lunchSetPrice = Menu.LunchSets.Where(x => x.Id == Guid.Parse(CheckedLunchSet))
               .Single()
               .Price;
           
           var orderForCreation = new OrderForCreationDto()
           {
               CustomerId = userId,
               MenuId = menuId,
               LunchSetId = Guid.Parse(CheckedLunchSet),
               Options = options
           };

           var totalPrice = optionsPrice + lunchSetPrice;
           
           var orderReturned = await _api.Orders_CreateOrderAsync(orderForCreation);
           var orderJson = JsonSerializer.Serialize(orderReturned);
           
           HttpContext.Session.SetString("OrderJson", orderJson);
           HttpContext.Session.SetString("TotalPrice", totalPrice.ToString());
           
           return RedirectToPage("/Order/SubmitOrder"); 
        }
        else
        {
            Menu = await _api.Menu_GetTodayMenuAsync();
            if (Menu is null)
            {
                return RedirectToPage("/Order/TodayMenuNotFound");
            }
        
            HttpContext.Session.SetString("MenuId", Menu.Id.ToString());
            return Page();
        }
        
    }
}