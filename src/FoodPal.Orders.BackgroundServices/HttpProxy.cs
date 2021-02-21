using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundServices
{
	internal class HttpProxy
	{
		private IDictionary<string, string> DefaultHeaders = new Dictionary<string, string> { { "Accept", "application/json" } };

		public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
		{
			var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
			{
				Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
			};

			foreach (var defaultHeader in DefaultHeaders)
				requestMessage.Headers.Add(defaultHeader.Key, defaultHeader.Value);

			try
			{
				using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
				{
					var responseMessage = await httpClient.SendAsync(requestMessage);
					return await HandleResponseMessage<TResponse>(responseMessage);
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"An error has occurred while sending POST request to Endpoint=[{endpoint}]", ex);
			}
		}

		private async Task<TResponse> HandleResponseMessage<TResponse>(HttpResponseMessage responseMessage)
		{
			if (!responseMessage.IsSuccessStatusCode)
			{
				var httpErrorMessage = await HandleUnsuccessfulResponseMessage(responseMessage);
				throw new Exception($"HTTP request failed with response content [{httpErrorMessage}]");
			}

			try
			{
				var contentAsString = await responseMessage.Content.ReadAsStringAsync();
				var responseObject = !string.IsNullOrEmpty(contentAsString) ? JsonConvert.DeserializeObject<TResponse>(contentAsString) : default;
				return responseObject;
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to read and deserialize HTTP response message.", ex);
			}
		}

		private async Task<string> HandleUnsuccessfulResponseMessage(HttpResponseMessage responseMessage)
		{
			try
			{
				return await responseMessage.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to read and deserialize HTTP unsuccessful response message.", ex);
			}
		}
	}
}
