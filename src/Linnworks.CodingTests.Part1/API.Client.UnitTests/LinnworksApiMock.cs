using Linnworks.CodingTests.Part1.Server.Common;
using System;
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
			SetCategoriesEndpoint();
			SetExecuteCustomScript();
		}

		public string Host => this.server.Urls[0];

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
						.WithBody(EmbeddedResourceHelpers
							.ReadFile("Linnworks.CodingTests.Part1.Server.API.Client.UnitTests.Responses.GetAllCategories.json")
				));
		}

		private void SetExecuteCustomScript()
		{
			this.server
				.Given(Request.Create().WithUrl(Host + "/" + Constants.ExecuteCustomScriptQueryUrl).UsingPost())
				.RespondWith(
					Response.Create()
						.WithStatusCode(200)
						.WithBody(EmbeddedResourceHelpers
							.ReadFile("Linnworks.CodingTests.Part1.Server.API.Client.UnitTests.Responses.ProductsCategorieCount.json")
				));
		}
	}
}
