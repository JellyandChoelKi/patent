using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2GGTT.ViewModels
{
    public class HotTechViewModel
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string ApplicantImgSrc { get; set; }
		public string ApplicantName { get; set; }
		public string ApplicantMajor { get; set; }
		public DateTime RegDate { get; set; }
		public DateTime UpdDate { get; set; }
	}
}
