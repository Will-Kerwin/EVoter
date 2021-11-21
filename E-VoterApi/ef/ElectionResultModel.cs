namespace E_VoterApi.Models
{
    public class ElectionResultModel
    {
        public Guid nomineeID {  get; set; }
        public Guid electionID {  get; set; }
        public int totalVotes {  get; set; }
        public int ranking { get; set; }
    }
}
