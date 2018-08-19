using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linnworks.CodingTests.Part1.Server.Services.Models
{
	public class Category
	{
		[JsonProperty("CategoryId")]
		public string Id { get; set; }

		[JsonProperty("CategoryName")]
		public string Name { get; set; }

		public int ProductsCount { get; set; }
	}
}
