using E_Voter.Models.Auth;

namespace E_Voter.Store.AuthUseCase.Actions
{
    public class LoginUserDataAction
    {
        public LoginUserDataAction(LoginDetailsModel credentials)
        {
            Credentials = credentials;
        }

        public LoginDetailsModel Credentials { get; }
    }
}
