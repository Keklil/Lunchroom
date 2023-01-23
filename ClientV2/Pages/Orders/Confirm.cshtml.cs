using System.Text.Json;
using ClientV2.Apis;
using ClientV2.Pages.Menu;
using ClientV2.Pages.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Orders;

public class Confirm : PageModel
{
    private readonly IApiClientV2 _api;
    private readonly IHttpContextAccessor _accessor;
    
    public string Qr { get; set; }
    public PaymentInfoDto PaymentInfo { get; set; }
    public string TotalPrice { get; set; }

    public Confirm(IApiClientV2 api, IHttpContextAccessor accessor)
    {
        _api = api;
        _accessor = accessor;
    }
    
    public async Task<ActionResult> OnGetAsync()
    {
        var cart = HttpContext.Session.GetString("cart");
        if (string.IsNullOrWhiteSpace(cart))
        {
            return Redirect("/Cart/Empty");
        }
        
        var group = await _api.Group_GetGroupAsync(new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        PaymentInfo = group.PaymentInfo;
        if (!string.IsNullOrWhiteSpace(PaymentInfo.Qr))
            PaymentInfo.Qr = string.Format("data:image/jpg;base64, {0}", PaymentInfo.Qr);

        var cartEntity = JsonSerializer.Deserialize<List<OrderContainer>>(cart);
        TotalPrice = HttpContext.Session.GetString("totalPrice");
        
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        var cart = HttpContext.Session.GetString("cart");
        if (string.IsNullOrWhiteSpace(cart))
        {
            return Redirect("/Cart/Empty");
        }
        
        var cartEntity = JsonSerializer.Deserialize<List<OrderContainer>>(cart);
        
        var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
        var userId = Guid.Parse(userIdFromContext);

        foreach (var orderDraft in cartEntity)
        {
            var optionsDto = new List<OrderOptionForCreationDto>();

            foreach (var optionDraft in orderDraft.OptionsIds)
            {
                var optionDto = new OrderOptionForCreationDto(){ OptionId = optionDraft, Units = 1};
                optionsDto.Add(optionDto);
            }
            
            var orderForCreation = new OrderForCreationDto()
            {
                CustomerId = userId,
                MenuId = orderDraft.MenuId,
                LunchSetId = orderDraft.LunchSetId,
                Options = optionsDto,
                LunchSetUnits = orderDraft.LunchSetUnits,
                GroupId = new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6")
            };
        
            var orderReturned = await _api.Orders_CreateOrderAsync(orderForCreation);
            await _api.Orders_ConfirmPaymentAsync(orderReturned.Id);
        }

        HttpContext.Session.Remove("cart");
        
        return Redirect("/Menu");
    }
}