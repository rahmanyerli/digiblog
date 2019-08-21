using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DigiBlog.Api.Models;

namespace DigiBlog.Api.Services
{
	public interface IArticleService
	{
		Task CreateAsync(Article article);
		Task UpdateAsync(Article article);
		Task DeleteAsync(string id);

		Task<IEnumerable<Article>> ReadAllAsync();
		Task<Article> ReadSingleAsync(string id);
		Task<IEnumerable<Article>> WhereAsync(Expression<Func<Article, bool>> where);
		Task<IEnumerable<Article>> OrderByAsync<TKey>(Expression<Func<Article, TKey>> orderBy, bool isDesc);
	}
}