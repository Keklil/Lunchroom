using ClientV2.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ClientV2.Pages.Team;

public class Index : PageModel
{
    private readonly IApiClientV2 _api;
    
    public TimeOnly? MenuExpired { get; set; }
    public GroupDto GroupInfo { get; set; }
    public List<UserDto> UserDtos { get; set; }
    public PaymentInfo PaymentInfo { get; set; }
    public Index(IApiClientV2 api)
    {
        _api = api;
    }
    
    public async Task<ActionResult> OnGetAsync()
    {
        var groupInfo = await _api.Group_GetGroupAsync(new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        GroupInfo = groupInfo;

        var list = new List<UserDto>();
        foreach (var id in groupInfo.Members)
        {
            var user = await _api.User_GetUserAsync(id);
            list.Add(user);
        }        
        UserDtos = list;

        PaymentInfo = new PaymentInfo();
        if (groupInfo.PaymentInfo is not null)
        {
            PaymentInfo.Description = groupInfo.PaymentInfo.Description;
            PaymentInfo.Link = groupInfo.PaymentInfo.Link;
        }

        if (groupInfo.Settings is not null)
        {
            MenuExpired = new TimeOnly(groupInfo.Settings.HourExpired, groupInfo.Settings.MinuteExpired);
        }

        return Page();
    }

    public async Task<ActionResult> OnGetPaymentModalPartialAsync()
    {
        var groupInfo = await _api.Group_GetGroupAsync(new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        var paymentInfo = new PaymentInfo();
        if (groupInfo.PaymentInfo is not null)
        {
            paymentInfo.Description = groupInfo.PaymentInfo.Description;
            paymentInfo.Link = groupInfo.PaymentInfo.Link;
        }
        return Partial("_PaymentModalPartial", paymentInfo);
    }
    
    public async Task<ActionResult> OnPostPaymentModalPartialAsync(PaymentInfo paymentInfo)
    {
        string qrBase64 = null;
        using (var memoryStream = new MemoryStream())
        {
            await paymentInfo.FormFile.CopyToAsync(memoryStream);
            
            if (memoryStream.Length < 1097152)
            {
                qrBase64 = Convert.ToBase64String(memoryStream.ToArray());
            }
            else
            {
                ModelState.AddModelError("File", "The file is too large.");
            }
        }

        var paymentDto = new PaymentInfoDto()
        {
            Description = paymentInfo.Description,
            Link = paymentInfo.Link,
            GroupId = new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"),
            Qr = qrBase64,
        };
        
        await _api.Group_ConfigurePaymentInfoAsync(paymentDto);

        return Partial("_PaymentModalPartial", paymentInfo);
    }
}