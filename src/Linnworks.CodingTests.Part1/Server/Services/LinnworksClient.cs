using Linnworks.CodingTests.Part1.Server.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Linnworks.CodingTests.Part1.Server.Services
{
	public class LinnworksClient
	{
		public LinnworksClient(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}

		public HttpClient HttpClient { get; }

		public async Task<IEnumerable<Category>> GetCategories()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "https://us.linnworks.net//api/Inventory/GetCategories");
			request.Headers.Add("Authorization", "43e40f4b-1935-4a68-be44-bb3028a81e69");
			var response = await HttpClient.SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<IEnumerable<Category>>(content);
			return result;
		}
	}
}
