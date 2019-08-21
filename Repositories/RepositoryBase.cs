using DigiBlog.Api.Models;

namespace DigiBlog.Api.Repositories
{
	public abstract class BaseRepository
	{
		protected readonly AppDbContext context;

		public BaseRepository(AppDbContext context)
		{
			this.context = context;
		}
	}
}