using E_Voter.Models.Auth;

namespace E_Voter.Store.AuthUseCase.Actions
{
    public class LoginUserDataActionResult
    {
        public LoginUserDataActionResult(bool isAuthenticated, UserModel user)
        {
            IsAuthenticated = isAuthenticated;
            User = user;
        }

        public bool IsAuthenticated { get; }
        public UserModel User { get; set; }
    }
}
