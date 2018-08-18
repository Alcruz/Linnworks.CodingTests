using Linnworks.CodingTests.Part3.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linnworks.CodingTests.Part3.Domain.Tests
{
	[TestClass]
	public class TestFindShortestPath
	{
		[TestMethod]
		public void FindShortestPath100Times100Matrix()
		{
			Random rnd = new Random();
			World world = new World(100,100);
			var path = world.FindShortestWay(new Location(0, 0), new Location(rnd.Next(0,100), rnd.Next(0,100)));
			Assert.IsTrue(path.Length > 0);
		}

		[TestMethod]
		public void FindShortestPath10Times10Matrix()
		{
			for (int i = 0; i < 100; i++)
			{
				World world = new World(10, 10);
				var path = world.FindShortestWay(new Location(0, 0), new Location(9, 9));
				Assert.IsTrue(path.Length > 0);
			}
		}
	}
}
