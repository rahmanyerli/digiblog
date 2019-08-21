using System.Reflection;
using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Repositories;
using DigiBlog.Api.Helpers;

namespace DigiBlog.Api.Services
{
	public class ArticleService : IArticleService
	{
		private readonly IRepository<Article> repository;

		public ArticleService(IRepository<Article> repository)
		{
			this.repository = repository;
		}

		public async Task CreateAsync(Article article)
		{
			if (string.IsNullOrWhiteSpace(article.Name))
			{
				throw new ArgumentNullException("Name");
			}
			else if (string.IsNullOrWhiteSpace(article.Text))
			{
				throw new ArgumentNullException("Text");
			}
			else if (string.IsNullOrWhiteSpace(article.CategoryId))
			{
				throw new ArgumentNullException("CategoryId");
			}
			else if (string.IsNullOrWhiteSpace(article.UserId))
			{
				throw new ArgumentNullException("UserId");
			}
			article.Id = GuidHelper.NewId();
			article.CreatedAt = DateTime.UtcNow;
			article.ModifiedAt = DateTime.UtcNow;
			article.Text = System.Net.WebUtility.HtmlEncode(article.Text);
			if (string.IsNullOrWhiteSpace(article.Status))
			{
				article.Status = Status.Active;
			}
			await repository.CreateAsync(article);
		}

		public async Task UpdateAsync(Article article)
		{
			var entity = await repository.ReadSingleAsync(article.Id);
			if (entity != null)
			{
				if (entity.UserId != article.UserId)
				{
					throw new AccessViolationException("You are not authorized to modify this record!");
				}
				if (!string.IsNullOrWhiteSpace(article.Name))
				{
					entity.Name = article.Name;
				}
				if (!string.IsNullOrWhiteSpace(article.Text))
				{
					entity.Text = System.Net.WebUtility.HtmlEncode(article.Text);
				}
				if (!string.IsNullOrWhiteSpace(article.CategoryId))
				{
					entity.CategoryId = article.CategoryId;
				}
				if (!string.IsNullOrWhiteSpace(article.Status))
				{
					entity.Status = article.Status;
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

		public async Task<IEnumerable<Article>> ReadAllAsync()
		{
			return await repository.ReadAllAsync();
		}

		public async Task<Article> ReadSingleAsync(string id)
		{
			var article = await repository.ReadSingleNestedAsync(id);
			return article;
		}

		public async Task<IEnumerable<Article>> WhereAsync(Expression<Func<Article, bool>> where)
		{
			return await repository.WhereAsync(where);
		}

		public async Task<IEnumerable<Article>> OrderByAsync<TKey>(Expression<Func<Article, TKey>> orderBy, bool isDesc)
		{
			return await repository.OrderByAsync(orderBy, isDesc);
		}
	}
}