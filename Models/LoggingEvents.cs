namespace DigiBlog.Api.Models
{
	public static class LoggingEvents
	{
		public const int GenerateItems = 1000;
		public const int ReadItems = 1001;
		public const int ReadItem = 1002;
		public const int ReadItemsBy = 1003;
		public const int CreateItem = 1004;
		public const int UpdateItem = 1005;
		public const int DeleteItem = 1006;

		public const int GetItemNotFound = 4000;
		public const int UpdateItemNotFound = 4001;
	}
}