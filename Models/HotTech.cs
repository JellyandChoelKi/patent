using System;

namespace K2GGTT.Models
{
	public class HotTech
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Overview { get; set; }
		public string BackgroundAndUnmetNeed { get; set; }
		public string TheSolution { get; set; }
		public string TechnologyEssence { get; set; }
		public string ApplicationsAndAdvantages { get; set; }
		public string DevelopmentStatus { get; set; }
		public string References { get; set; }
		public string PatentStatus { get; set; }
		public string ApplicantImgSrc { get; set; }
		public string ApplicantName { get; set; }
		public string ApplicantMajor { get; set; }
		public DateTime RegDate { get; set; }
		public DateTime UpdDate { get; set; }
	}
}
