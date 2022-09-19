using System;
using System.Collections.Generic;

namespace MVcInterview.Models.CandidateDetailsT
{
    public partial class CandidateDetailsT
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public string CandidateExperience { get; set; } = null!;
        public string CandidatePhoneNo { get; set; } = null!;
        public string CandidateMailId { get; set; } = null!;
        public string CandidateSkills { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
