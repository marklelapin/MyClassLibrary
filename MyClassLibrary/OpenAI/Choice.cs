namespace MyClassLibrary.OpenAI
{
	public class Choice
	{
		public Message message { get; set; } = new Message();
		public int? index { get; set; } = null;
		public string finish_Reason { get; set; } = "";

	}
}
