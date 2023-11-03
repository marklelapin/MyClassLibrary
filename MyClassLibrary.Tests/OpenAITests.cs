using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyClassLibrary.Interfaces;
using MyClassLibrary.OpenAI;
using System.Reflection;
using System.Text.Json;
using Xunit.Sdk;

namespace MyClassLibrary.Tests
{
	public class OpenAITests
	{
		private readonly IOpenAIControllerService _openAIControllerService;


		public OpenAITests()
		{

			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

			//ConfigureServices() for testing of OpenAIControllerService which requires IHttpClientFactory and IConfiguration.
			//This is more integration testing than unit testing but it is useful to have, is run on ad Hoc basis and is readonly in regards to external services.
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddUserSecrets<OpenAITests>()
				.Build();

			var services = new ServiceCollection();

			services.AddHttpClient("OpenAI", opts =>
			{
				opts.BaseAddress = new Uri(configuration.GetValue<string>("OpenAIApi:BaseUrl")!);
				opts.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration.GetValue<string>("OpenAIApi:ApiKey")}");
			});
			services.AddSingleton<IConfiguration>(configuration);
			services.AddTransient<IOpenAIControllerService, OpenAIControllerService>();

			var serviceProvider = services.BuildServiceProvider();

			_openAIControllerService = serviceProvider.GetRequiredService<IOpenAIControllerService>();

		}

		public static readonly object[][] chatCompletionRequestConstructorTestData =
		{
			new object[]{"greet user","Hi my name is Jim","gpt-3.5-turbo",3883},
			new object[]{"","Hi my name is Jim","gpt-3.5-turbo",3886},
			new object[]{"greet user","","gpt-3.5-turbo",3888},
			new object[]{"greet user","Hi my name is Jim","gpt-4",7773,(Action<ChatCompletionRequestOptions>)(options => { options.Model = "gpt-4"; }) },
			new object[]{"greet user","Hi my name is Jim","gpt-3.5-turbo",1000,(Action<ChatCompletionRequestOptions>)(options => { options.Max_Tokens = 1000; }) },
		};

		[Theory, MemberData(nameof(chatCompletionRequestConstructorTestData))]
		public void ChatCompletionRequestConstructorTest(string systemPrompt, string userPrompt, string expectedModel, int expectedMaxTokens, Action<ChatCompletionRequestOptions>? options = null)
		{
			ChatCompletionRequest chatCompletionRequest = new ChatCompletionRequest(systemPrompt, userPrompt, options);


			Assert.Equal(expectedModel, chatCompletionRequest.model);
			Assert.Equal(expectedMaxTokens, chatCompletionRequest.max_tokens);
			Assert.Equal(2, chatCompletionRequest.messages.Count);
			Assert.Equal(systemPrompt.Length, chatCompletionRequest.messages[0].content.Length);
			Assert.Equal(userPrompt.Length, chatCompletionRequest.messages[1].content.Length);

		}


		public static readonly object[][] getChatCompletionContentData =
		{
			new object[]{"greet user","Hi my name is Jim",200},
			new object[]{"100#words","sdfsf",200},
			new object[]{"2000#words","DFsfs",200 },
			new object[]{"3000#words","DFsfs",400 },

		};
		[Theory, MemberData(nameof(getChatCompletionContentData))]
		public async Task GetChatCompletionContentTest(string systemPrompt, string userPrompt, int expectedStatusCode, Action<ChatCompletionRequestOptions>? configureOptions = null)
		{
			if (systemPrompt.Contains("#words"))
			{
				int targetWordCount = int.Parse(systemPrompt.Substring(0, systemPrompt.IndexOf("#words")));
				systemPrompt = GeneratedWords(targetWordCount);
			}

			var content = await _openAIControllerService.GetChatCompletionContent(systemPrompt, userPrompt, configureOptions);


			if (content is ObjectResult objectResult)
			{
				Assert.Equal(expectedStatusCode, objectResult.StatusCode);
				Assert.Equal(typeof(string), objectResult.Value.GetType());
				Assert.True(((string)objectResult.Value).Length > 0);
				JsonSerializer.Deserialize<object>((string)objectResult.Value);
				//Assert.Equal("hello", ((string)objectResult.Value));
				//Assert.NotEmpty((string)objectResult.Value);
			}
			else
			{
				throw new XunitException("Test failed as content is not an ObjectResult.");
			}


		}





		private string GeneratedWords(int targetWordCount)
		{

			var faker = new Faker();
			int currentWordCount = 0;
			string randomText = "";

			while (currentWordCount < targetWordCount)
			{
				string sentence = faker.Lorem.Sentence();
				int wordCount = sentence.Split(' ').Length;

				if (currentWordCount + wordCount <= targetWordCount)
				{
					randomText += sentence + " ";
					currentWordCount += wordCount;
				}
				else
				{
					// If adding the sentence exceeds the word count, break the loop.
					break;
				}
			}
			return randomText;

		}




	}
}
