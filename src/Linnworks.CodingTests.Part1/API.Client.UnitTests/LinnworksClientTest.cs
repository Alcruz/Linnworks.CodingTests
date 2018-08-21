using System;
using Xunit;

namespace Linnworks.CodingTests.Part1.Server.API.Client.UnitTests
{
	public class LinnworksClientTest
	{
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
	}
}
