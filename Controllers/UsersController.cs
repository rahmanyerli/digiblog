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
    public class UsersController : ControllerBase
	{
		private readonly IUserService service;
		private readonly ILogger logger;

		public UsersController(IUserService service, ILogger<CategoriesController> logger)
		{
			this.service = service;
			this.logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> ReadAll()
		{
			try
			{
				var user = await service.ReadAllAsync();
				return Ok(user);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItems, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> ReadSingle(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				logger.LogError(LoggingEvents.ReadItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				var user = await service.ReadSingleAsync(id);
				return Ok(user);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.ReadItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<ActionResult<User>> Create([FromBody] User user)
		{
			if (!ModelState.IsValid)
			{
				logger.LogError(LoggingEvents.CreateItem, "Invalid request!");
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.CreateAsync(user);
				return CreatedAtAction(nameof(ReadSingle), new { id = user.Id }, user);
			}
			catch (Exception ex)
			{
				logger.LogError(LoggingEvents.CreateItem, ex, ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Update(string id, [FromBody] User user)
		{
			if (!ModelState.IsValid || id != user.Id || string.IsNullOrWhiteSpace(id))
			{
				return BadRequest("Invalid request!");
			}
			try
			{
				await service.UpdateAsync(user);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(string id)
		{
			try
			{
				await service.DeleteAsync(id);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		public IActionResult Authenticate([FromBody]User user)
		{
			User authenticatedUser;
			try
			{
				if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
				{
					return BadRequest(new { message = "Username or password is invalid!" });
				}

				authenticatedUser = service.Authenticate(user.Username, user.Password);

				if (authenticatedUser == null)
				{
					return BadRequest(new { message = "Username or password is invalid!" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok(authenticatedUser);
		}
	}
}
