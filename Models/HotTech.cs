using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace K2GGTT.Models
{
	public class HotTech
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		[NotMapped]
		public IFormFile ApplicantImg { get; set; }
		public string ApplicantImgSrc { get; set; }
		public string ApplicantName { get; set; }
		public string ApplicantMajor { get; set; }
		public DateTime RegDate { get; set; }
		public DateTime UpdDate { get; set; }
	}
}
