namespace backend.Core.Dtos.Candidate
{
    public class CandidateGetDto
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CoverLetter { get; set; }
        public string ResumeUrl { get; set; }
        public long JobId { get; set; }

        // This property will be retrieved by joining with the Jobs table
        public string JobTitle { get; set; }
    }
}