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

		private async Task<ExecuteCustomScriptResult<ProductCategoryCount>> GetProductCategoryCount()
		{
			return await ExecuteCustomScript<ProductCategoryCount>(EmbeddedResourceHelpers.ReadFile("Linnworks.CodingTests.Part1.Server.Services.CustomScripts.ProductCategoriesCount.sql"));
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
			var request = new HttpRequestMessage(HttpMethod.Post, uri);
			request.Headers.Add("Authorization", "43e40f4b-1935-4a68-be44-bb3028a81e69");
			if (body != null)
			{
				request.Content = new FormUrlEncodedContent(body);
			}

			var response = await HttpClient.SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<T>(content);
			return result;
		}
	}
}
