namespace E_VoterApi.Models
{
    public class NomineeModel
    {

        public Guid electionID { get; set; }
        public Guid userID { get; set; }
        public string association { get; set; }
        public string description { get; set; }
    }
}
