namespace MyClassLibrary.OpenAI
{
	public class Usage
	{
		public int prompt_tokens { get; set; } = 0;
		public int completion_tokens { get; set; } = 0;
		public int total_tokens { get; set; } = 0;
	}
}
