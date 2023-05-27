using Microsoft.AspNetCore.SignalR;

namespace Otus.Teaching.PromoCodeFactory.SignalR
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var httpContext = connection.GetHttpContext();
            var username = httpContext.Request.Query["username"].ToString();
            return username;
        }
    }
}
