namespace E_VoterApi.Models
{
    public class UserDetails
    {
        public Guid? userID {  get; set; }
        public string email { get; set; }
        public string? password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string contactNo { get; set; }
    }
}

