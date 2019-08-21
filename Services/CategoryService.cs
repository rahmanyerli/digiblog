using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Repositories;
using System.Linq.Expressions;
using DigiBlog.Api.Helpers;

namespace DigiBlog.Api.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IRepository<Category> repository;

		public CategoryService(IRepository<Category> repository)
		{
			this.repository = repository;
		}

		public async Task CreateAsync(Category category)
		{
			category.Id = GuidHelper.NewId();
			category.CreatedAt = DateTime.UtcNow;
			category.ModifiedAt = DateTime.UtcNow;
			if (string.IsNullOrWhiteSpace(category.Status))
			{
				category.Status = Status.Active;
			}
			await repository.CreateAsync(category);
		}

		public async Task UpdateAsync(Category category)
		{
			var entity = await repository.ReadSingleAsync(category.Id);
			if (entity != null)
			{
				if (!string.IsNullOrWhiteSpace(category.Name))
				{
					entity.Name = category.Name;
				}
				if (!string.IsNullOrWhiteSpace(category.Status))
				{
					entity.Status = category.Status;
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

		public async Task<IEnumerable<Category>> ReadAllAsync()
		{
			return await repository.ReadAllAsync();
		}

		public async Task<Category> ReadSingleAsync(string id)
		{
			return await repository.ReadSingleNestedAsync(id);
		}

		public async Task<IEnumerable<Category>> WhereAsync(Expression<Func<Category, bool>> where)
		{
			return await repository.WhereAsync(where);
		}

		public async Task<IEnumerable<Category>> OrderByAsync<TKey>(Expression<Func<Category, TKey>> orderBy, bool isDesc)
		{
			return await repository.OrderByAsync(orderBy, isDesc);
		}
	}
}