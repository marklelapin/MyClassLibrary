namespace MyClassLibrary.OpenAI
{
	public class ChatCompletionRequestOptions
	{
		public string Model { get; set; } = "gpt-3.5-turbo";

		public int? Max_Tokens { get; set; } = null;

		public float Temperature { get; set; } = 0;

		public float Top_P { get; set; } = 1;

		public float Frequency_Penalty { get; set; } = 0;

		public float Presence_Penalty { get; set; } = 0;

	}
}
