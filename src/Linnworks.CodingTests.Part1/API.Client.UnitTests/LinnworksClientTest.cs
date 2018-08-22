using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Linnworks.CodingTests.Part1.Server.API.Client.UnitTests
{
	public class LinnworksClientTest : IClassFixture<LinnworksApiMock>
	{
		private readonly LinnworksApiMock linnworksApiMock;
		private readonly string baseUrl;
		private readonly string authSession;

		public LinnworksClientTest(LinnworksApiMock linnworksApiMock)
		{
			this.linnworksApiMock = linnworksApiMock;
			this.baseUrl = linnworksApiMock.Host;
			this.authSession = "00000-00000-0000";
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("", "")]
		[InlineData("\t\t", "\t\t")]
		public void TestConstructor(string baseUrl, string authSession)
		{
			Assert.Throws<InvalidOperationException>(() =>
			{
				var linnworksClient = new LinnworksClient(baseUrl, authSession);
			});
		}

		[Fact]
		public async Task TestGetAllCategories()
		{
			const int expectedCategoriesCount = 4;
			const string expectedFirstCategoryName = "Default";
			const int expectedFirstProductCount = 10;

			var linnworksClient = new LinnworksClient(this.baseUrl, this.authSession);
			var result = (await linnworksClient.GetCategories()).ToList();
			
			Assert.Equal(expectedCategoriesCount, result.Count);
			Assert.Equal(expectedFirstCategoryName, result[0].Name);
			Assert.Equal(expectedFirstProductCount, result[0].ProductsCount);
		}
	}
}
