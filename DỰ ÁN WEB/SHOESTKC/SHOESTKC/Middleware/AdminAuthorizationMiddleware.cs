using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SHOESTKC.Middleware
{
    public class AdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Kiểm tra nếu đang truy cập vào Admin Controller
            if (path != null && path.StartsWith("/admin"))
            {
                var userRole = context.Session.GetString("UserRole");
                var userId = context.Session.GetInt32("UserId");

                // Nếu chưa đăng nhập hoặc không phải admin
                if (userId == null || userRole != "admin")
                {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}