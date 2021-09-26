namespace E_Voter.Models.Auth
{
    public class UserModel
    {
        public Guid userID { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string contactNo { get; set; }
    }
}
