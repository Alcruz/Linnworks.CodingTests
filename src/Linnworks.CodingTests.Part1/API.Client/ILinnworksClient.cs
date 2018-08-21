using System.Collections.Generic;
using System.Threading.Tasks;
using Linnworks.CodingTests.Part1.Server.API.Client.Models;

namespace Linnworks.CodingTests.Part1.Server.API.Client
{
	public interface ILinnworksClient
	{
		Task<Category> CreateCategory(string categoryName);
		Task DeleteCategory(string categoryId);
		Task<IEnumerable<Category>> GetCategories();
	}
}