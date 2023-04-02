using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Client.Apis;

public partial class ApiClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;
    private string _token;

    partial void PrepareRequest(HttpClient client, HttpRequestMessage request,
        StringBuilder urlBuilder)
    {
        if (_contextAccessor is not null)
        {
            var tokenSession = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Token")?.Value;
            if (tokenSession is not null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenSession);
        }
    }

    partial void ProcessResponse(HttpClient client, HttpResponseMessage response)
    {
        var status = (int)response.StatusCode;
        if (status == 401) throw new AuthException();
    }

    public ApiClient(HttpClient httpClient, IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _contextAccessor = contextAccessor;
        _httpClient = httpClient;
        _settings = new Lazy<JsonSerializerOptions>(CreateSerializerSettings);
        _configuration = configuration;
        BaseUrl = _configuration.GetSection("ApiUrl").Value;
    }
}

public class AuthException : Exception
{
}