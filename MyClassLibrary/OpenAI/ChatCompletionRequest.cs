using SharpToken;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyClassLibrary.OpenAI
{
	public class ChatCompletionRequest
	{

		//Default values for these properties are set in the constructor and come from ChatCompletionRequestOptions

		public List<Message> messages { get; set; } = new List<Message>();

		public string model { get; set; }

		public int? max_tokens { get; set; }

		public float temperature { get; set; }

		public float top_p { get; set; }

		public float frequency_penalty { get; set; }

		public float presence_penalty { get; set; }

		[JsonIgnore]
		public int? Timeout { get; set; } = null;

		[JsonIgnore]
		public int RetryAttempts { get; set; } = 0;

		private Dictionary<string, int> ModelTokenLimits = new Dictionary<string, int>
		{
			{ "gpt-3.5-turbo",4096 }
			,{"gpt-4",8191 }
		};


		private double MaxTokenBuffer = 0.05;

		public ChatCompletionRequest(string systemPrompt, string userPrompt, Action<ChatCompletionRequestOptions>? configureOptions = null)
		{
			ChatCompletionRequestOptions _options = new ChatCompletionRequestOptions();
			if (configureOptions != null)
			{
				configureOptions(_options);
			}



			model = _options.Model;
			max_tokens = _options.Max_Tokens ?? CalculatedMaxTokens(model, systemPrompt, userPrompt);
			temperature = _options.Temperature;
			top_p = _options.Top_P;
			frequency_penalty = _options.Frequency_Penalty;
			presence_penalty = _options.Presence_Penalty;

			var _systemMessage = new Message()
			{
				role = "system",
				content = systemPrompt
			};

			var _userMessage = new Message()
			{
				role = "user",
				content = userPrompt
			};

			messages.Add(_systemMessage);
			messages.Add(_userMessage);

		}

		public string ToJson()
		{
			return JsonSerializer.Serialize(this);
		}

		private int CalculatedMaxTokens(string model, string systemPrompt, string userPrompt)
		{

			int modelTokenLimit;

			if (ModelTokenLimits.TryGetValue(model, out modelTokenLimit) == false)
			{
				modelTokenLimit = 4096;
			}

			var encoding = GptEncoding.GetEncodingForModel(model);

			var encodedPrompts = encoding.Encode(systemPrompt + userPrompt);

			var promptTokens = encodedPrompts.Count;

			var maxTokens = (int)(modelTokenLimit * (1 - MaxTokenBuffer));

			return Math.Max(20, maxTokens - promptTokens);

		}



	}
}
