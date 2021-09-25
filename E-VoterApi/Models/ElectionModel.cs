namespace E_VoterApi.Models
{
    public class ElectionModel
    {
        public Guid? elctionID { get; set; }
        public DateTime electionStartDate { get; set; }
        public DateTime electionEndDate { get; set; }
        public string electionName { get; set; }
    }
}
