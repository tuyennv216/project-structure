using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Configs
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext,
			Exception exception,
			CancellationToken cancellationToken)
		{
			// 1. Ghi log lỗi chi tiết để lập trình viên kiểm tra (có thể dùng Serilog)
			_logger.LogError(exception, "Lỗi không xử lý phát sinh tại đường dẫn: {Path}. Thông điệp: {Message}",
				httpContext.Request.Path, exception.Message);

			// 2. Định nghĩa cấu trúc trả về chuẩn (ProblemDetails)
			var statusCode = exception switch
			{
				KeyNotFoundException => StatusCodes.Status404NotFound,
				UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
				ArgumentException => StatusCodes.Status400BadRequest,
				_ => StatusCodes.Status500InternalServerError
			};

			var problemDetails = new ProblemDetails
			{
				Status = statusCode,
				Title = statusCode == StatusCodes.Status500InternalServerError
					? "Lỗi hệ thống nội bộ."
					: "Yêu cầu không hợp lệ.",
				Detail = exception.Message, // (Lưu ý: Môi trường Production nên ẩn chi tiết lỗi hệ thống)
				Instance = httpContext.Request.Path
			};

			httpContext.Response.StatusCode = statusCode;

			// 3. Trả về kết quả dạng JSON cho client
			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			// Trả về true để báo hiệu exception đã được xử lý xong, không bị truyền đi nữa
			return true;
		}
	}
}
