using System;

namespace DigiBlog.Api.Helpers
{
	public class GuidHelper
	{
		public static string NewId()
		{
			var guid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
			return guid;
		}
	}
}