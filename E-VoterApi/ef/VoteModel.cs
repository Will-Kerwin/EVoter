namespace E_VoterApi.Models
{
    public class VoteModel
    {
        public Guid? voteID {  get; set; }
        public Guid electionID {  get; set; }
        public Guid nomineeID {  get; set; }
        public Guid voterID {  get; set; }
    }
}
