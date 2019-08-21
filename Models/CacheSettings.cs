using System;
using Microsoft.Extensions.Caching.Memory;

namespace DigiBlog.Api.Models
{
	public class CacheSettings
	{
		public double AbsoluteExpiration { get; set; }
		public double SlidingExpiration { get; set; }

		public MemoryCacheEntryOptions GetOptions()
		{
			var options = new MemoryCacheEntryOptions();
			options.SetAbsoluteExpiration(TimeSpan.FromSeconds(AbsoluteExpiration));
			options.SetSlidingExpiration(TimeSpan.FromSeconds(SlidingExpiration));
			return options;
		}
	}
}