namespace DigiBlog.Api.Models
{
	public class AuthSettings
	{
		public string Secret { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
        public double Expires { get; set; }
    }
}