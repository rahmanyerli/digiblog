using System.Diagnostics;
using System.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DigiBlog.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService service;
		private readonly ILogger logger;
		private readonly IMemoryCache cache;
		private readonly MemoryCacheEntryOptions cacheOptions;

		public CategoriesController(ICategoryService service, ILogger<CategoriesController> logger, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
		{
			this.service = service;
			this.logger = logger;
			this.cache = cache;
			this.cacheOptions = cacheSettings.Value.GetOptions();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Category>>> ReadAll()
		{
			try
			{
				IEnumerable<Category> categories;
				if (!cache.TryGetValue("Categories.ReadAll", out categories))
				{
					categories = await service.ReadAllAsync();
					cache.Set("Categories.ReadAll", categories, cacheOptions);
				}
				return Ok(categories);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItems, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Category>> ReadSingle(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.ReadItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				var category = await service.ReadSingleAsync(id);
				return Ok(category);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<ActionResult<Category>> Create([FromBody] Category category)
		{
			if (!ModelState.IsValid)
			{
				logger.LogError(LoggingEvents.CreateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.CreateAsync(category);
				return CreatedAtAction(nameof(ReadSingle), new { id = category.Id }, category);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.CreateItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Update(string id, [FromBody] Category category)
		{
			if (!ModelState.IsValid || id != category.Id || string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.UpdateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.UpdateAsync(category);
				return Ok();
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.UpdateItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.DeleteItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.DeleteAsync(id);
				return Ok();
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.DeleteItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}
	}
}
