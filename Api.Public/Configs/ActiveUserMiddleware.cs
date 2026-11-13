using Core.Metadata.Names;
using Core.Metadata.Options;

namespace Api.Public.Configs
{
	public class ActiveUserMiddleware
	{
		private readonly RequestDelegate _next;

		public ActiveUserMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IActiveUser loggedUser)
		{
			// Kiểm tra xem user đã được xác thực chưa (đã qua [Authorize] hoặc token hợp lệ)
			if (context.User?.Identity?.IsAuthenticated == true)
			{
				// Trích xuất claims sử dụng ClaimNames của bạn
				var userIdStr = context.User.FindFirst(ClaimNames.UserId)?.Value;
				var emailStr = context.User.FindFirst(ClaimNames.Email)?.Value;
				var permissionsStr = context.User.FindFirst(ClaimNames.Permissions)?.Value;

				if (userIdStr == null || emailStr == null || permissionsStr == null)
				{
					await _next(context);
					return;
				}

				// Gán vào Service để dùng xuyên suốt request
				if (loggedUser is ActiveUser mutableUser)
				{
					mutableUser.UserId = Guid.Parse(userIdStr);
					mutableUser.Email = emailStr;
					mutableUser.Permissions = permissionsStr.Split(';');
				}
			}

			// Chuyển request sang middleware tiếp theo trong pipeline
			await _next(context);
		}
	}
}
