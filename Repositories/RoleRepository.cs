using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigiBlog.Api.Models;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace DigiBlog.Api.Repositories
{
	public class RoleRepository : BaseRepository, IRepository<Role>
	{
		public RoleRepository(AppDbContext context) : base(context)
		{
		}

		public async Task CreateAsync(Role role)
		{
			await context.Role.AddAsync(role);
			await context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Role role)
		{
			context.Role.Update(role);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Role role)
		{
			context.Role.Remove(role);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Role>> ReadAllAsync()
		{
			return await context.Role.ToListAsync();
		}

		public async Task<Role> ReadSingleAsync(string id)
		{
			return await context.Role
				.SingleOrDefaultAsync(role => role.Id == id);
		}

		public async Task<Role> ReadSingleNestedAsync(string id)
		{
			return await context.Role
				.SingleOrDefaultAsync(role => role.Id == id);
		}

		public async Task<IEnumerable<Role>> WhereAsync(Expression<Func<Role, bool>> where)
		{
			return await context.Role
				.Where(where)
				.ToListAsync();
		}

		public async Task<IEnumerable<Role>> OrderByAsync<TKey>(Expression<Func<Role, TKey>> orderBy, bool isDesc)
		{
			if (isDesc)
			{
				return await context.Role
					.OrderByDescending(orderBy)
					.ToListAsync();
			}
			else
			{
				return await context.Role
					.OrderBy(orderBy)
					.ToListAsync();
			}
		}
	}
}