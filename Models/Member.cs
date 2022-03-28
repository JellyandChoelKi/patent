using System;

namespace K2GGTT.Models
{
	public class Member
	{
		public int Id { get; set; }
		public string MemberId { get; set; }
		public string Password { get; set; }
		public int Gubun { get; set; }
		public string Name { get; set; }
		public string CompanyRegistrationNumber { get; set; }
		public string Representative { get; set; }
		public string TaxInvoiceOfficer { get; set; }
		public string Contact { get; set; }
		public string Zipcode { get; set; }
		public string Addr1 { get; set; }
		public string Addr2 { get; set; }
		public string Email { get; set; }
		public string Jobtitle { get; set; }
		public string Website { get; set; }
		public string Country { get; set; }
	}
}
