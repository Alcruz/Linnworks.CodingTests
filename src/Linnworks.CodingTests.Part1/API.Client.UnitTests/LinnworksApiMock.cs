using Linnworks.CodingTests.Part1.Server.API.Client.Models;
using Linnworks.CodingTests.Part1.Server.Common;
using Linnworks.CodingTests.Part1.Server.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Linnworks.CodingTests.Part1.Server.API.Client.UnitTests
{
	public class LinnworksApiMock : IDisposable
	{
		private FluentMockServer server;

		public LinnworksApiMock()
		{
			this.server = FluentMockServer.Start();

			Categories = JsonConvert
				.DeserializeObject<List<TestCategory>>(
					EmbeddedResourceHelpers.ReadFile("Linnworks.CodingTests.Part1.Server.API.Client.UnitTests.Responses.GetAllCategories.json"));
			ProductCategoryCount = JsonConvert
				.DeserializeObject<ExecuteCustomScriptResult<ProductCategoryCount>>(
					EmbeddedResourceHelpers.ReadFile("Linnworks.CodingTests.Part1.Server.API.Client.UnitTests.Responses.ProductsCategorieCount.json"));

			SetCategoriesEndpoint();
			SetExecuteCustomScript();
			SetUpAddCategory();
		}

		public string Host => this.server.Urls[0];
		public List<TestCategory> Categories { get; }
		public ExecuteCustomScriptResult<ProductCategoryCount> ProductCategoryCount { get; }

		public TestCategory FindCategoryById(string categoryId)
		{
			return Categories.Find(category => category.Id == categoryId);
		}

		public TestCategory FindCategoryByName(string categoryName)
		{
			return Categories.Find(category => category.Name == categoryName);
		}

		public void Dispose()
		{
			this.server.Dispose();
		}

		private void SetCategoriesEndpoint()
		{
			this.server
				.Given(Request.Create().WithUrl(Host + "/" + Constants.GetCategoriesUrl).UsingPost())
				.RespondWith(
					Response.Create()
						.WithStatusCode(200)
						.WithBody(request => JsonConvert.SerializeObject(Categories))
				);
		}

		private void SetExecuteCustomScript()
		{
			this.server
				.Given(Request.Create().WithUrl(Host + "/" + Constants.ExecuteCustomScriptQueryUrl).UsingPost())
				.RespondWith(
					Response.Create()
						.WithStatusCode(200)
						.WithBody(request => JsonConvert.SerializeObject(ProductCategoryCount))
				);
		}

		private void SetUpAddCategory()
		{
			this.server
				.Given(Request.Create().WithUrl(Host + "/" + Constants.CreateCategoryUrl).UsingPost())
				.RespondWith(
					Response.Create()
					.WithCallback(CreateCategory)
					.WithStatusCode(200)
				);
		}

		private WireMock.ResponseMessage CreateCategory(WireMock.RequestMessage request)
		{
			string body = request.Body;
			var regex = new Regex("categoryName=(?<categoryName>\\w*)");
			var match = regex.Match(body);
			var categoryName = match.Groups["categoryName"].Value;
			var createdCategory = new TestCategory
			{
				Id = Guid.NewGuid().ToString(),
				Name = categoryName
			};
			Categories.Add(createdCategory);
			return new WireMock.ResponseMessage { BodyAsJson = createdCategory };
		}

		public class TestCategory
		{
			[JsonProperty("CategoryId")]
			public string Id { get; set; }

			[JsonProperty("CategoryName")]
			public string Name { get; set; }
		}
	}
}
