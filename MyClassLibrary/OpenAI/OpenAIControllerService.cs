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
			int lastStatusCode = 0;

			for (int i = 0; i <= chatCompletionRequest.RetryAttempts; i++)
			{
				var responseContent = await getResponseContent(chatCompletionRequest.ToJson(), chatCompletionRequest.Timeout);

				try
				{
					if (responseContent is ObjectResult objectResult)

						if (objectResult.StatusCode == 200)
						{
							string chatContent = getChatContent((string)objectResult.Value);
							return Ok(chatContent);
						}
						else if (objectResult.StatusCode != 408 && objectResult.StatusCode != 503 && objectResult.StatusCode != 500) //timeout or sevice unavailable codes
						{
							lastStatusCode = (int)objectResult.StatusCode;
							return StatusCode((int)objectResult.StatusCode, (string)objectResult.Value);
						};
				}
				catch
				{
					return StatusCode(500, "Failed to read response content from OpenAI API reponse.");
				}

			}

			return StatusCode(503, $"Aborted OpenAI request after {chatCompletionRequest.RetryAttempts} retry attempts. Status Code {lastStatusCode}");
		}

		private async Task<IActionResult> getResponseContent(string httpContentJson, int? timeout = null)
		{

			var httpContent = new StringContent(httpContentJson, Encoding.UTF8, "application/json");

			var client = _factory.CreateClient(ServiceName);
			using (var cts = new CancellationTokenSource())
			{
				if (timeout != null)
				{
					cts.CancelAfter((int)timeout * 1000);
				}
				try
				{
					HttpResponseMessage response = await client.PostAsync("", httpContent, cts.Token);

					var responseContent = response.Content.ReadAsStringAsync().Result;

					return StatusCode((int)response.StatusCode, responseContent);
				}
				catch (TaskCanceledException)
				{
					return StatusCode(408, $"Request to {ServiceName} API timed out.");
				}
				catch (Exception ex)
				{
					return StatusCode(500, $"Failed to read response content from {ServiceName} API. {ex.Message}");
				}
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




