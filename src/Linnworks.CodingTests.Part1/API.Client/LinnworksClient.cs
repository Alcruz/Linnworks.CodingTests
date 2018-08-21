using Linnworks.CodingTests.Part1.Server.API.Client.Models;
using Linnworks.CodingTests.Part1.Server.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Linnworks.CodingTests.Part1.Server.API.Client
{
	public class LinnworksClient
	{
		public LinnworksClient()
		{
			HttpClient = new HttpClient();
		}

		public HttpClient HttpClient { get; }

		public async Task<IEnumerable<Category>> GetCategories()
		{
			var categories = await SendRequest<IEnumerable<Category>>("https://us.linnworks.net//api/Inventory/GetCategories");
			ExecuteCustomScriptResult<ProductCategoryCount> productsCount = await GetProductCategoryCount();
			var productsCountDict = productsCount.Results.ToDictionary(x => x.CategoryId, x => x.ProductsCount);
			return categories.Select(category => new Category
			{
				Id = category.Id,
				Name = category.Name,
				ProductsCount = productsCountDict.ContainsKey(category.Id) ? productsCountDict[category.Id] : 0
			});
		}

		public async Task DeleteCategory(string categoryId)
		{
			var response = await SendRequest("https://us.linnworks.net//api/Inventory/DeleteCategoryById", new Dictionary<string, string>
			{
				{ "categoryId", categoryId }
			});

			if (response.IsSuccessStatusCode)
				return;


			throw new InvalidOperationException();
		}

		private async Task<ExecuteCustomScriptResult<ProductCategoryCount>> GetProductCategoryCount()
		{
			return await ExecuteCustomScript<ProductCategoryCount>(EmbeddedResourceHelpers.ReadFile("Linnworks.CodingTests.Part1.Server.API.Client.CustomScripts.ProductCategoriesCount.sql"));
		}

		private Task<ExecuteCustomScriptResult<TResult>> ExecuteCustomScript<TResult>(string customScript)
		{
			return SendRequest<ExecuteCustomScriptResult<TResult>>("https://us.linnworks.net//api/Dashboards/ExecuteCustomScriptQuery", new Dictionary<string, string>
			{
				{ "script", customScript}
			});
		}

		private async Task<T> SendRequest<T>(string uri, Dictionary<string, string> body = null)
		{
			var response = await SendRequest(uri, body);
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<T>(content);
			return result;
		}

		private async Task<HttpResponseMessage> SendRequest(string uri, Dictionary<string, string> body = null)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, uri);
			request.Headers.Add("Authorization", "de259865-33b2-4f0c-a9c1-82958fb32cc9");
			if (body != null)
			{
				request.Content = new FormUrlEncodedContent(body);
			}

			return await HttpClient.SendAsync(request);
		}
	}
}
