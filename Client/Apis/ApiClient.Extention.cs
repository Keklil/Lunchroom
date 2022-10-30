using System.Net.Http.Headers;

namespace Client.Apis;

public partial class ApiClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;
    private string _token;
    public ApiClient(System.Net.Http.HttpClient httpClient, IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _contextAccessor = contextAccessor;
        _httpClient = httpClient;
        _settings = new System.Lazy<System.Text.Json.JsonSerializerOptions>(CreateSerializerSettings);
        _configuration = configuration;
        BaseUrl = _configuration.GetSection("ApiUrl").Value;
    }

    partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request,
        System.Text.StringBuilder urlBuilder)
    {
        if (_contextAccessor is not null)
        {
            var tokenSession = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Token")?.Value;
            if (tokenSession is not null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenSession);
        }
    }

    partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response)
    {
        var status = (int)response.StatusCode;
        if (status == 401)
        {
            throw new AuthException();
        }
    }
}

public class AuthException : Exception
{
    
}