using ClientV2.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IApiClientV2 _api;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        private int hour, minute;

        public List<MenuForList> Menus { get; set; }
        public List<OrdersForUser>? UserOrders { get; set; }
        public List<OrderDto>? TodayUserOrders { get; set; }

        public bool TimeExpiredMessage { get; set; }
        public bool TimeExpired { get; set; }
        
        [BindProperty]
        public string MenuDate { get; set; }
        [BindProperty]
        public string OrderId { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger, IApiClientV2 api, IHttpContextAccessor accessor,
            IConfiguration configuration)
        {
            _logger = logger;
            _api = api;
            _accessor = accessor;
            _configuration = configuration;

            hour = _configuration.GetValue<int>("TimeMenuExpiration:Hour");
            minute = _configuration.GetValue<int>("TimeMenuExpiration:Minute");
        }

        public async Task<ActionResult> OnGetAsync()
        {
            if (DateTime.Now.TimeOfDay > new TimeSpan(hour, minute, 0))
            {
                TimeExpired = true;
            }
            
            await GetData();

            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (HttpContext.User.IsInRole("admin"))
            {
                var date = MenuDate;
                return Redirect("./Report/OrdersReport?date=" + date);
            }

            if (HttpContext.User.IsInRole("user"))
            {
                
            }

            return RedirectToPage("/Index");
        }

        public async Task<ActionResult> OnPostDeleteAsync(string orderId)
        {
            if (DateTime.Now.TimeOfDay > new TimeSpan(hour, minute, 0))
            {
                TimeExpiredMessage = true;
                await GetData();
                return Page();
            }
            
            var orderIdGuid = Guid.Parse(orderId);

            await _api.Orders_DeleteOrderAsync(orderIdGuid);

            await GetData();
            
            return Page();
        }

        public async Task GetData()
        {
            if (HttpContext.User.IsInRole("user") || HttpContext.User.IsInRole("admin"))
            {
                var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
                var userId = Guid.Parse(userIdFromContext);
                UserOrders = await _api.Orders_GetTodayOrdersByUserAsync(userId, Guid.NewGuid()) as List<OrdersForUser>;
                TodayUserOrders = new List<OrderDto>();
                foreach (var order in UserOrders.OrderByDescending(x=>x.Date))
                {
                    var todayOrder = await _api.Orders_GetOrderAsync(order.Id);
                    TodayUserOrders.Add(todayOrder);
                }
            }
        }
    }
}