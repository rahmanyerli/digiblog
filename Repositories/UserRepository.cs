using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigiBlog.Api.Models;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace DigiBlog.Api.Repositories
{
	public class UserRepository : BaseRepository, IRepository<User>
	{
		public UserRepository(AppDbContext context) : base(context)
		{
		}

		public async Task CreateAsync(User user)
		{
			await context.User.AddAsync(user);
			await context.SaveChangesAsync();
		}

		public async Task UpdateAsync(User user)
		{
			context.User.Update(user);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(User user)
		{
			context.User.Remove(user);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<User>> ReadAllAsync()
		{
			return await context.User.ToListAsync();
		}

		public async Task<User> ReadSingleAsync(string id)
		{
			return await context.User
				.SingleOrDefaultAsync(user => user.Id == id);
		}

		public async Task<User> ReadSingleNestedAsync(string id)
		{
			return await context.User
				.Include(user => user.Role)
				.SingleOrDefaultAsync(user => user.Id == id);
		}

		public async Task<IEnumerable<User>> WhereAsync(Expression<Func<User, bool>> where)
		{
			return await context.User
				.Where(where)
				.Include(user => user.Role)
				.ToListAsync();
		}

		public async Task<IEnumerable<User>> OrderByAsync<TKey>(Expression<Func<User, TKey>> orderBy, bool isDesc)
		{
			if (isDesc)
			{
				return await context.User
					.OrderByDescending(orderBy)
					.ToListAsync();
			}
			else
			{
				return await context.User
					.OrderBy(orderBy)
					.ToListAsync();
			}
		}
	}
}