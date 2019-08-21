using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DigiBlog.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin,Author,Member")]
	public class CommentsController : ControllerBase
	{
		private readonly ICommentService service;
		private readonly ILogger logger;
		private readonly IMemoryCache cache;
		private readonly MemoryCacheEntryOptions cacheOptions;

		public CommentsController(ICommentService service, ILogger<CommentsController> logger, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
		{
			this.service = service;
			this.logger = logger;
			this.cache = cache;
			this.cacheOptions = cacheSettings.Value.GetOptions();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Comment>>> ReadAll()
		{
			try
			{
				IEnumerable<Comment> comments;
				if (!cache.TryGetValue("Comments.ReadAll", out comments))
				{
					comments = await service.ReadAllAsync();
					cache.Set("Comments.ReadAll", comments, cacheOptions);
				}
				return Ok(comments);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItems, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("[action]/{articleId}")]
		[ActionName("ByArticle")]
		public async Task<ActionResult<IEnumerable<Article>>> ReadByArticle(string articleId)
		{
			if (string.IsNullOrWhiteSpace(articleId))
			{
				logger.LogError(LoggingEvents.ReadItemsBy, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				IEnumerable<Comment> comments;
				if (!cache.TryGetValue($"Comments.ReadByArticle.{articleId}", out comments))
				{
					comments = await service.WhereAsync(c => c.ArticleId == articleId);
					cache.Set($"Comments.ReadByArticle.{articleId}", comments, cacheOptions);
				}
				return Ok(comments);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItemsBy, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Comment>> ReadSingle(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.ReadItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				var comment = await service.ReadSingleAsync(id);
				return Ok(comment);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<ActionResult<Comment>> Create([FromBody] Comment comment)
		{
			if (!ModelState.IsValid)
			{
				logger.LogError(LoggingEvents.CreateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.CreateAsync(comment);
				return CreatedAtAction(nameof(ReadSingle), new { id = comment.Id }, comment);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.CreateItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Update(string id, [FromBody] Comment comment)
		{
			if (!ModelState.IsValid || id != comment.Id || string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.UpdateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.UpdateAsync(comment);
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
