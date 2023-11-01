

using Microsoft.AspNetCore.Mvc.Formatters;

namespace MyClassLibrary.Configuration
{
	/// <summary>
	/// Allows text/plain content type to be used in mvc controllers.
	/// </summary>
	/// <remarks>
	/// needs to be added in program.cs:
	/// services.addControllers(options => options.InputFormatters.Insert(options.InputFormatters.Count, new TextPlainInputFormatter()));
	/// https://peterdaugaardrasmussen.com/2020/02/29/asp-net-core-how-to-make-a-controller-endpoint-that-accepts-text-plain/
	/// </remarks>
	public class TextPlainInputFormatter : InputFormatter
	{
		private const string ContentType = "text/plain";

		public TextPlainInputFormatter()
		{
			SupportedMediaTypes.Add(ContentType);
		}

		public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
		{
			var request = context.HttpContext.Request;
			using (var reader = new StreamReader(request.Body))
			{
				var content = await reader.ReadToEndAsync();
				return await InputFormatterResult.SuccessAsync(content);
			}
		}

		public override bool CanRead(InputFormatterContext context)
		{
			var contentType = context.HttpContext.Request.ContentType ?? "";
			return contentType.StartsWith(ContentType);
		}
	}
}
