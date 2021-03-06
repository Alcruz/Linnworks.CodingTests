﻿using Linnworks.CodingTests.Part1.Server.API.Client.Models;
using Linnworks.CodingTests.Part1.Server.Common;
using Linnworks.CodingTests.Part1.Server.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Linnworks.CodingTests.Part1.Server.API.Client
{
	public class LinnworksClient : ILinnworksClient
	{
		public LinnworksClient(string baseUrl, string authSession)
		{
			if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(authSession))
				throw new InvalidOperationException();

			HttpClient = new HttpClient(new AuthSessionHandler(authSession) { InnerHandler = new HttpClientHandler() });
			HttpClient.BaseAddress = new Uri(baseUrl);
		}

		public HttpClient HttpClient { get; }

		public async Task<IEnumerable<Category>> GetCategories()
		{
			var categories = await SendRequest<IEnumerable<Category>>(Constants.GetCategoriesUrl);
			ExecuteCustomScriptResult<ProductCategoryCount> productsCount = await GetProductCategoryCount();
			var productsCountDict = productsCount.Results.ToDictionary(x => x.CategoryId, x => x.ProductsCount);
			return categories.Select(category => new Category
			{
				Id = category.Id,
				Name = category.Name,
				ProductsCount = productsCountDict.ContainsKey(category.Id) ? productsCountDict[category.Id] : 0
			});
		}

		public async Task<Category> CreateCategory(string categoryName)
		{
			var category = await SendRequest<Category>(Constants.CreateCategoryUrl, new Dictionary<string, string>
			{
				{ "categoryName", categoryName }
			});

			return category;
		}

		public async Task DeleteCategory(string categoryId)
		{
			var response = await SendRequest(Constants.DeleteCategoryByIdUrl, new Dictionary<string, string>
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
			return SendRequest<ExecuteCustomScriptResult<TResult>>(Constants.ExecuteCustomScriptQueryUrl, new Dictionary<string, string>
			{
				{ "script", customScript}
			});
		}

		private async Task<T> SendRequest<T>(string uri, Dictionary<string, string> body = null)
		{
			var response = await SendRequest(uri, body);
			if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				throw new LinnworksBadRequestException(JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync()));
			else if (!response.IsSuccessStatusCode)
				throw new InvalidOperationException();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<T>(content);
			return result;
		}

		private async Task<HttpResponseMessage> SendRequest(string uri, Dictionary<string, string> body = null)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, uri);
			if (body != null)
			{
				request.Content = new FormUrlEncodedContent(body);
			}

			return await HttpClient.SendAsync(request);
		}

		private class AuthSessionHandler : MessageProcessingHandler
		{
			public string AuthSession { get; }

			public AuthSessionHandler(string authSession)
			{
				AuthSession = authSession;
			}

			protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				request.Headers.Add("Authorization", new string[] { AuthSession });
				return request;
			}

			protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
			{
				return response;
			}
		}
	}
}
