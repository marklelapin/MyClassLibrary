using System.Net;
using System.Text.Json;

namespace MyClassLibrary.Extensions;


public static class TaskHttpMessageExtensions
{

	public static (T? responseObject, HttpStatusCode statusCode) ConvertTo<T>(this Task<HttpResponseMessage> responseMessage)
	{
		T? responseObject;

		(string responseBody
		 , HttpStatusCode statusCode) = responseMessage.GetResponseData();
		(string requestUri
			, string requestMessage) = responseMessage.GetRequestData();

		responseObject = GetResponseObject<T>(responseBody, statusCode, requestUri, requestMessage);


		return (responseObject, statusCode);
	}



	public static async Task<(T? responseObject, HttpStatusCode statusCode)> ConvertToAsync<T>(this Task<HttpResponseMessage> responseMessage)
	{
		T? responseObject;

		(string responseBody
		, HttpStatusCode statusCode) = await GetResponseDataAsync(responseMessage);
		(string requestUri
			, string requestMessage) = await GetRequestDataAsync(responseMessage);

		responseObject = GetResponseObject<T>(responseBody, statusCode, requestUri, requestMessage);

		return (responseObject, statusCode);
	}




	public static (string responseBody, HttpStatusCode statusCode) GetResponseData(this Task<HttpResponseMessage> responseMessage)
	{
		responseMessage.Wait();
		Task<string> getResponseBody = responseMessage.Result.Content.ReadAsStringAsync();
		getResponseBody.Wait();
		string responseBody = getResponseBody.Result;
		HttpStatusCode statusCode = responseMessage.Result.StatusCode;
		return (responseBody, statusCode);
	}

	public static (string requestUri, string requestMessage) GetRequestData(this Task<HttpResponseMessage> responseMessage)
	{
		string requestMessage;
		string requestUri;

		responseMessage.Wait();
		try
		{
			Task<string> getRequestMessage = responseMessage.Result.RequestMessage!.Content!.ReadAsStringAsync();
			getRequestMessage.Wait();
			requestMessage = getRequestMessage.Result;
			requestUri = responseMessage.Result.RequestMessage.RequestUri!.OriginalString;
		}
		catch
		{
			requestMessage = "Request Message not available";
			requestUri = "Uri not available";

		}

		return (requestUri, requestMessage);
	}


	public static async Task<(string responseBody, HttpStatusCode statusCode)> GetResponseDataAsync(this Task<HttpResponseMessage> responseMessage)
	{
		using var response = await responseMessage;
		string body = await response.Content.ReadAsStringAsync();
		HttpStatusCode statusCode = response.StatusCode;

		return (body, statusCode);
	}

	//TODO - re think TaskHttpMessageExtensions. GetResponseDataAsync above does what the others should be doing.
	//commented out code below as failing.
	public static async Task<(string requestUri, string requestMessage)> GetRequestDataAsync(this Task<HttpResponseMessage> responseMessage)
	{



		string requestMessage;
		string requestUri;
		//try
		//{        
		//    requestUri = responseMessage.Result.RequestMessage!.RequestUri!.OriginalString;
		//    requestMessage = await responseMessage.Result.RequestMessage.Content!.ReadAsStringAsync();
		//} catch 
		//{
		requestMessage = "Request Message not available";
		requestUri = "Uri not available";

		//}
		return (requestUri, requestMessage);
	}

	private static T? GetResponseObject<T>(string responseBody, HttpStatusCode statusCode, string requestUri, string requestMessage)
	{
		T? output;

		if (statusCode == HttpStatusCode.NotFound)
		{
			output = default(T);
		}
		else if (statusCode == HttpStatusCode.OK)
		{
			try
			{
				output = JsonSerializer.Deserialize<T>(responseBody)!;
			}
			catch (Exception ex)
			{

				throw new Exception("Output from HttpResponseMessage" + responseBody
					+ "cannot be converted to an object of type " + typeof(T).Name + "\n"
					+ "Output = \"" + responseBody + "\"", ex);
			}
		}
		else
		{

			throw new Exception("HttpResponseMesssage from api(" + requestUri + ")"
								+ "returned following status code: " + statusCode.ToString() + ".\n"
								+ "ResponseBody: " + responseBody);
		}

		return output;
	}

}
