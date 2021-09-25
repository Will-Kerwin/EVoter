namespace E_VoterApi.Models
{
    public class LoginModel
    {
        public Guid? userID {  get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
