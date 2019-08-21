using System.Linq;
using System.Buffers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DigiBlog.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin,Author,Member")]
	public class ArticlesController : ControllerBase
	{
		private readonly IArticleService service;
		private readonly ILogger logger;
		private readonly IMemoryCache cache;
		private readonly MemoryCacheEntryOptions cacheOptions;

		public ArticlesController(IArticleService service, ILogger<ArticlesController> logger, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
		{
			this.service = service;
			this.logger = logger;
			this.cache = cache;
			this.cacheOptions = cacheSettings.Value.GetOptions();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Article>>> ReadAll()
		{
			try
			{
				IEnumerable<Article> articles;
				if (!cache.TryGetValue("Articles.ReadAll", out articles))
				{
					articles = await service.ReadAllAsync();
					cache.Set("Articles.ReadAll", articles, cacheOptions);
				}
				return Ok(articles);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItems, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("[action]/{categoryId}")]
		[ActionName("ByCategory")]
		public async Task<ActionResult<IEnumerable<Article>>> ReadByCategory(string categoryId)
		{
			if (string.IsNullOrWhiteSpace(categoryId))
			{
				logger.LogError(LoggingEvents.ReadItemsBy, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				IEnumerable<Article> articles;
				if (!cache.TryGetValue($"Articles.ReadByCategory.{categoryId}", out articles))
				{
					articles = await service.WhereAsync(a => a.CategoryId == categoryId);
					cache.Set($"Articles.ReadByCategory.{categoryId}", articles, cacheOptions);
				}
				return Ok(articles);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItemsBy, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Article>> ReadSingle(string id)
		{

			if (string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.ReadItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				var article = await service.ReadSingleAsync(id);
				return Ok(article);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("[action]/{keyword}")]
		[ActionName("Search")]
		public async Task<ActionResult<IEnumerable<Article>>> Search(string keyword)
		{
			if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 3)
			{
				logger.LogError(LoggingEvents.ReadItemsBy, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				IEnumerable<Article> articles;
				if (!cache.TryGetValue($"Articles.Search.{keyword}", out articles))
				{
					articles = await service.WhereAsync(a => a.Name.ToLower().Contains(keyword.ToLower()) || a.Text.ToLower().Contains(keyword.ToLower()));
					cache.Set($"Articles.Search.{keyword}", articles, cacheOptions);
				}
				return Ok(articles);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItemsBy, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Author")]
		public async Task<ActionResult<Article>> Create([FromBody] Article article)
		{
			if (!ModelState.IsValid)
			{
				logger.LogError(LoggingEvents.CreateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.CreateAsync(article);
				return CreatedAtAction(nameof(ReadSingle), new { id = article.Id }, article);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.CreateItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin,Author")]
		public async Task<ActionResult> Update(string id, [FromBody] Article article)
		{
			if (!ModelState.IsValid || id != article.Id || string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.UpdateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.UpdateAsync(article);
				return Ok();
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.UpdateItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
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
