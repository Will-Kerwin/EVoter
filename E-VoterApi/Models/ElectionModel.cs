namespace E_VoterApi.Models
{
    public class ElectionModel
    {
        public Guid? electionID { get; set; }
        public DateTime electionStartDate { get; set; }
        public DateTime electionEndDate { get; set; }
        public string electionName { get; set; }
        public int? totalVotes {  get; set; }
        public Guid? winnerID {  get; set; }
        public long modifiedTicks { get; set; }
    }
}
