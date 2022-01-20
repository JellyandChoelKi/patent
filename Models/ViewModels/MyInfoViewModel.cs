using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace K2GGTT.ViewModels
{
	public class MyInfoViewModel
	{
		public int Id { get; set; }
		public string MemberId { get; set; }
		public string Password { get; set; }
		[NotMapped]
		public string CurrentPassword { get; set; }
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
	}
}
