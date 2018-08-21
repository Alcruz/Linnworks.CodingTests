using Linnworks.CodingTests.Part1.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Linnworks.CodingTests.Part1.Server.API.Client;

namespace Linnworks.CodingTests.Part1.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		public CategoriesController()
		{
			LinnWorksClient = new LinnworksClient();
		}

		public LinnworksClient LinnWorksClient { get; }

		[HttpGet]
		public async Task<ActionResult<IEnumerable<object>>> GetAllAsync()
		{
			var categories = await LinnWorksClient.GetCategories();
			return Ok(categories.Select(category => new { 
				category.Id,
				category.Name,
				category.ProductsCount
			}));
		}

		[HttpDelete("{categoryId}")]
		public async Task<ActionResult> Delete(string categoryId)
		{
			await LinnWorksClient.DeleteCategory(categoryId);
			return Ok();
		}
	}
}
