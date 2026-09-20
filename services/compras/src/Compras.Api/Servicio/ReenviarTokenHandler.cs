using System.Net.Http.Headers;

namespace Compras.Api.Servicio;

public class ReenviarTokenHandler : DelegatingHandler
{
    private readonly string keyAccessToken = "access_token_ezkart";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReenviarTokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies[keyAccessToken];

        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, cancellationToken);
    }
}
