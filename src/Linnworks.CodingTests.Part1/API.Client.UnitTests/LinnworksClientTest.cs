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
			const string expectedFirstCategoryName = "Default";
			const int expectedFirstProductCount = 10;

			const string expectedLastCategoryName = "New Category";
			const int expectedLastProductCount = 0;

			var linnworksClient = new LinnworksClient(this.baseUrl, this.authSession);
			var result = (await linnworksClient.GetCategories()).ToList();
			var defaultCategory = result.Find(r => r.Name == "Default");
			var newCategory = result.Find(r => r.Name == "New Category");


			Assert.Equal(linnworksApiMock.Categories.Count, result.Count);
			Assert.Equal(expectedFirstCategoryName, defaultCategory.Name);
			Assert.Equal(expectedFirstProductCount, defaultCategory.ProductsCount);
			Assert.Equal(expectedLastCategoryName, newCategory.Name);
			Assert.Equal(expectedLastProductCount, newCategory.ProductsCount);
		}

		[Theory]
		[InlineData("Category1")]
		[InlineData("Category2")]
		public async Task TestAddValidCategory(string categoryName)
		{
			var linnworksClient = new LinnworksClient(this.baseUrl, this.authSession);
			var category = await linnworksClient.CreateCategory(categoryName);

			Assert.NotNull(linnworksApiMock.FindCategoryById(category.Id));
		}
	}
}
