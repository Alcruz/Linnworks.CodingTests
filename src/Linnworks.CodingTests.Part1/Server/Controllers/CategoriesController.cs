using Linnworks.CodingTests.Part1.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Linnworks.CodingTests.Part1.Server.API.Client;
using System;
using Linnworks.CodingTests.Part1.Server.API.Client.Models;

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
			return Ok(categories.Select(category => new 
			{
				category.Id,
				category.Name,
				category.ProductsCount
			}));
		}

		[HttpPost]
		public async Task<ActionResult<object>> CreateAsync(CreateCategory model) 
		{
			var category = await model.Create(LinnWorksClient);
			return Ok(new 
			{ 
				category.Id,
				category.Name
			});
		}

		[HttpDelete("{categoryId}")]
		public async Task<ActionResult> DeleteAsync(string categoryId)
		{
			await LinnWorksClient.DeleteCategory(categoryId);
			return NoContent();
		}

		public class CreateCategory
		{
			public string Name { get; set; }

			internal Task<Category> Create(LinnworksClient linnWorksClient)
			{
				if (Name == null)
					throw new NullReferenceException(Name);

				return linnWorksClient.CreateCategory(Name);
			}
		}
	}
}
