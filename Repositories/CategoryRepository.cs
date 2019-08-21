using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigiBlog.Api.Models;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace DigiBlog.Api.Repositories
{
	public class CategoryRepository : BaseRepository, IRepository<Category>
	{
		public CategoryRepository(AppDbContext context) : base(context)
		{
		}

		public async Task CreateAsync(Category category)
		{
			await context.Category.AddAsync(category);
			await context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Category category)
		{
			context.Category.Update(category);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Category category)
		{
			context.Category.Remove(category);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Category>> ReadAllAsync()
		{
			return await context.Category.ToListAsync();
		}

		public async Task<Category> ReadSingleAsync(string id)
		{
			return await context.Category
				.SingleOrDefaultAsync(category => category.Id == id);
		}

		public async Task<Category> ReadSingleNestedAsync(string id)
		{
			return await context.Category
				.SingleOrDefaultAsync(category => category.Id == id);
		}

		public async Task<IEnumerable<Category>> WhereAsync(Expression<Func<Category, bool>> where)
		{
			return await context.Category
				.Where(where)
				.OrderBy(category => category.Name)
				.ToListAsync();
		}

		public async Task<IEnumerable<Category>> OrderByAsync<TKey>(Expression<Func<Category, TKey>> orderBy, bool isDesc)
		{
			if (isDesc)
			{
				return await context.Category
					.OrderByDescending(orderBy)
					.ToListAsync();
			}
			else
			{
				return await context.Category
					.OrderBy(orderBy)
					.ToListAsync();
			}
		}
	}
}