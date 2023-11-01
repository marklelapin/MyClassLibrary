using Microsoft.AspNetCore.Mvc;
using MyClassLibrary.OpenAI;

namespace MyClassLibrary.Interfaces
{
	public interface IOpenAIControllerService
	{
		/// <summary>
		/// Return an OpenAI chat completion string from the OpenAI API
		/// </summary>
		/// <param name="systemPrompt"></param>
		/// <param name="userPrompt"></param>
		/// <param name="configureOptions"></param>
		/// <returns></returns>
		Task<IActionResult> GetChatCompletionContent(string systemPrompt, string userPrompt, Action<ChatCompletionRequestOptions>? configureOptions);
	}


}
