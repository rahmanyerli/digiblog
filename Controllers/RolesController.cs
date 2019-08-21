using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigiBlog.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
	{
		private readonly IRoleService service;
		private readonly ILogger logger;

		public RolesController(IRoleService service, ILogger<RolesController> logger)
		{
			this.service = service;
			this.logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Role>>> ReadAll()
		{
			try
			{
				var role = await service.ReadAllAsync();
				return Ok(role);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItems, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Role>> ReadSingle(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.ReadItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				var role = await service.ReadSingleAsync(id);
				return Ok(role);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<ActionResult<Role>> Create([FromBody] Role role)
		{
			if (!ModelState.IsValid)
			{
				logger.LogError(LoggingEvents.CreateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.CreateAsync(role);
				return CreatedAtAction(nameof(ReadSingle), new { id = role.Id }, role);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.CreateItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Update(string id, [FromBody] Role role)
		{
			if (!ModelState.IsValid || id != role.Id || string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.UpdateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.UpdateAsync(role);
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
