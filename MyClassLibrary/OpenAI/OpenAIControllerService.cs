using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyClassLibrary.Interfaces;
using System.Text;
using System.Text.Json;

namespace MyClassLibrary.OpenAI
{
	public class OpenAIControllerService : ControllerBase, IOpenAIControllerService
	{
		private readonly IHttpClientFactory _factory;
		private readonly string _openAIKey;
		private readonly string ServiceName = "OpenAI";

		public OpenAIControllerService(IHttpClientFactory factory, IConfiguration config)
		{
			_factory = factory;
			_openAIKey = config.GetValue<string>("OpenAIKey");
		}

		public async Task<IActionResult> GetChatCompletionContent(string systemPrompt, string userPrompt, Action<ChatCompletionRequestOptions>? configureOptions = null)
		{
			var chatCompletionRequest = new ChatCompletionRequest(systemPrompt, userPrompt, configureOptions);

			var responseContent = await getResponseContent(chatCompletionRequest.ToJson());

			if (responseContent is ObjectResult objectResult)
			{
				string chatContent = getChatContent((string)objectResult.Value);
				return StatusCode((int)objectResult.StatusCode, chatContent);
			}

			return StatusCode(500, "Failed to read response content from OpenAI API reponse.");

		}

		private async Task<IActionResult> getResponseContent(string httpContentJson)
		{
			try
			{
				var httpContent = new StringContent(httpContentJson, Encoding.UTF8, "application/json");

				var client = _factory.CreateClient(ServiceName);

				HttpResponseMessage response = await client.PostAsync("", httpContent);

				var responseContent = response.Content.ReadAsStringAsync().Result;

				return StatusCode((int)response.StatusCode, responseContent);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Failed to read response content from {ServiceName} API. {ex.Message}");
			}
		}

		private string getChatContent(string responseContent)
		{
			string? chatCompletionContent = null;

			try
			{
				ChatCompletionResponse? chatCompletionResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseContent);
				if ((chatCompletionResponse?.choices.Count ?? 0) == 0) { throw new Exception(); };

				chatCompletionContent = chatCompletionResponse.Content();

			}
			catch
			{
				try
				{
					ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
					if (errorResponse != null)
					{
						chatCompletionContent = errorResponse.error.message;
					}
				}
				catch { }
			}

			chatCompletionContent = chatCompletionContent ?? "Failed to deserialize error message or chatContent from OpenAI API response.";
			return chatCompletionContent;
		}
	}
}




