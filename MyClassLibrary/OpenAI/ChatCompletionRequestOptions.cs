namespace MyClassLibrary.OpenAI
{
	public class ChatCompletionRequestOptions
	{
		/// <summary>
		/// Chat GPT model to use
		/// </summary>
		public string Model { get; set; } = "gpt-3.5-turbo";

		/// <summary>
		/// The max number of tokens to generate. If blank will estimate the max possible.
		/// </summary>
		public int? Max_Tokens { get; set; } = null;

		/// <summary>
		/// The randomness of the output. 0 = no randomness. 1 = full randomness
		/// </summary>
		public float Temperature { get; set; } = 0;

		/// <summary>
		/// The percentage likelihood that the model will choose the "best" answer. 0 = no likelihood. 1 = full likelihood
		/// </summary>
		public float Top_P { get; set; } = 1;


		public float Frequency_Penalty { get; set; } = 0;

		public float Presence_Penalty { get; set; } = 0;

		/// <summary>
		/// No of retry attempts if the request fails (e.g. due to rate limiting, openAI 503 etc timeouts.)
		/// </summary>
		public int? RetryAttempts { get; set; } = null;

		/// <summary>
		/// The time in seconds before the request times out.
		/// </summary>
		public int? Timeout { get; set; } = null;

	}
}
