namespace MyClassLibrary.OpenAI
{
	public class ChatCompletionResponse
	{
		public string id { get; set; } = "";

		public string object_ { get; set; } = "chat.completion";

		public int? created { get; set; } = null;

		public string model { get; set; } = "";

		public List<Choice> choices { get; set; } = new List<Choice>();
		public Usage usage { get; set; } = new Usage();



		public string Content()
		{
			return choices[0].message.content;
		}

	}





}
