using System;
using System.ComponentModel.DataAnnotations;

namespace DigiBlog.Api.Models
{
	public class Role
	{
		[Key]
		[MaxLength(32)]
		public string Id { get; set; }

		[MaxLength(100)]
		public string Name { get; set; }

		[DataType(DataType.Date)]
		public DateTime CreatedAt { get; set; }

		[DataType(DataType.Date)]
		public DateTime ModifiedAt { get; set; }

		[MaxLength(1)]
		public string Status { get; set; }
	}
}