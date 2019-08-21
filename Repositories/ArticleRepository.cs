using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigiBlog.Api.Models;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace DigiBlog.Api.Repositories
{
	public class ArticleRepository : BaseRepository, IRepository<Article>
	{
		public ArticleRepository(AppDbContext context) : base(context)
		{
		}

		public async Task CreateAsync(Article article)
		{
			await context.Article.AddAsync(article);
			await context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Article article)
		{
			context.Article.Update(article);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Article article)
		{
			context.Article.Remove(article);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Article>> ReadAllAsync()
		{
			return await context.Article.ToListAsync();
		}

		public async Task<Article> ReadSingleAsync(string id)
		{
			return await context.Article
				.SingleOrDefaultAsync(article => article.Id == id);
		}

		public async Task<Article> ReadSingleNestedAsync(string id)
		{
			return await context.Article
				.Include(article => article.Category)
				.Include(article => article.User)
				.Include(article => article.Comments)
					.ThenInclude(comment => comment.User)
				.SingleOrDefaultAsync(article => article.Id == id);
		}

		public async Task<IEnumerable<Article>> WhereAsync(Expression<Func<Article, bool>> where)
		{
			return await context.Article
				.Where(where)
				.ToListAsync();
		}

		public async Task<IEnumerable<Article>> OrderByAsync<TKey>(Expression<Func<Article, TKey>> orderBy, bool isDesc)
		{
			if (isDesc)
			{
				return await context.Article
					.OrderByDescending(orderBy)
					.ToListAsync();
			}
			else
			{
				return await context.Article
					.OrderBy(orderBy)
					.ToListAsync();
			}
		}
	}
}