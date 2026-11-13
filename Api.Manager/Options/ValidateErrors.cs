using FluentValidation.Results;

namespace Api.Manager.Options
{
	public static class ValidateErrors
	{
		public static object Get(ValidationResult result)
		{
			return new
			{
				Code = 400,
				Errors = result.Errors.Select(e => new
				{
					Field = e.PropertyName,
					Message = e.ErrorMessage
				})
			};
		}
	}
}
