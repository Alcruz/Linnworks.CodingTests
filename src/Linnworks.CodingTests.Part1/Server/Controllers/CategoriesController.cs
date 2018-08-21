using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Linnworks.CodingTests.Part1.Server.API.Client;
using Linnworks.CodingTests.Part1.Server.API.Client.Models;

namespace Linnworks.CodingTests.Part1.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		public CategoriesController(LinnworksClient linnworksClient)
		{
			LinnworksClient = linnworksClient;
		}

		public LinnworksClient LinnworksClient { get; }

		[HttpGet]
		public async Task<ActionResult<IEnumerable<object>>> GetAllAsync()
		{
			var categories = await LinnworksClient.GetCategories();
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
			var category = await model.Create(LinnworksClient);
			return Ok(new 
			{ 
				category.Id,
				category.Name
			});
		}

		[HttpDelete("{categoryId}")]
		public async Task<ActionResult> DeleteAsync(string categoryId)
		{
			await LinnworksClient.DeleteCategory(categoryId);
			return NoContent();
		}

		public class CreateCategory
		{
			public string CategoryName { get; set; }

			internal Task<Category> Create(LinnworksClient LinnworksClient)
			{
				if (CategoryName == null)
					throw new NullReferenceException(CategoryName);

				return LinnworksClient.CreateCategory(CategoryName);
			}
		}
	}
}
