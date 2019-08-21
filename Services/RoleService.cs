using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Repositories;
using System.Linq.Expressions;
using DigiBlog.Api.Helpers;

namespace DigiBlog.Api.Services
{
	public class RoleService : IRoleService
	{
		private readonly IRepository<Role> repository;

		public RoleService(IRepository<Role> repository)
		{
			this.repository = repository;
		}

		public async Task CreateAsync(Role role)
		{
			role.Id = GuidHelper.NewId();
			role.CreatedAt = DateTime.UtcNow;
			role.ModifiedAt = DateTime.UtcNow;
			if (string.IsNullOrWhiteSpace(role.Status))
			{
				role.Status = Status.Active;
			}
			await repository.CreateAsync(role);
		}

		public async Task UpdateAsync(Role role)
		{
			var entity = await repository.ReadSingleAsync(role.Id);
			if (entity != null)
			{
				if (!string.IsNullOrWhiteSpace(role.Name))
				{
					entity.Name = role.Name;
				}
				if (!string.IsNullOrWhiteSpace(role.Status))
				{
					entity.Status = role.Status;
				}
				entity.ModifiedAt = DateTime.UtcNow;
				await repository.UpdateAsync(entity);
			}
			else
			{
				throw new KeyNotFoundException("No data found!");
			}
		}

		public async Task DeleteAsync(string id)
		{
			var entity = await repository.ReadSingleAsync(id);
			if (entity != null)
			{
				await repository.DeleteAsync(entity);
			}
			else
			{
				throw new KeyNotFoundException("No data found!");
			}
		}

		public async Task<IEnumerable<Role>> ReadAllAsync()
		{
			return await repository.ReadAllAsync();
		}

		public async Task<Role> ReadSingleAsync(string id)
		{
			return await repository.ReadSingleNestedAsync(id);
		}

		public async Task<IEnumerable<Role>> WhereAsync(Expression<Func<Role, bool>> where)
		{
			return await repository.WhereAsync(where);
		}

		public async Task<IEnumerable<Role>> OrderByAsync<TKey>(Expression<Func<Role, TKey>> orderBy, bool isDesc)
		{
			return await repository.OrderByAsync(orderBy, isDesc);
		}
	}
}