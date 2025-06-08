using AspNetCoreHero.ToastNotification.Abstractions;

namespace InternetShopAspNetCoreMvc.UI.Middleware
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, INotyfService notifyService)
	    : IMiddleware
    {
	    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next(context);
                notifyService.Error("sss");
            }
			catch (Exception ex)
			{
				var message = ex.Message;
                logger.LogInformation(message);
            }
        }
    }
}
