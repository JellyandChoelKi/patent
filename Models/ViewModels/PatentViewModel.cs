using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2GGTT.ViewModels
{
    public class PatentViewModel
    {
        public List<string> TitleList { get; set; }

        public List<string> ApplicatnNoList { get; set; }

        public List<string> PublicationNumberList { get; set; }

        public List<string> PublicationDateList { get; set; }

        public List<string> ApplicantNameList { get; set; }

        public List<string> CpcList { get; set; }

        public List<string> DescriptionList { get; set; }

        public List<string> ImgSrcList { get; set; }
    }
}
