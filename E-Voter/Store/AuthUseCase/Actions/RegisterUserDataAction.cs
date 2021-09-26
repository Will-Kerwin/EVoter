using E_Voter.Models.Auth;

namespace E_Voter.Store.AuthUseCase.Actions
{
    public class RegisterUserDataAction
    {
        public RegisterUserDataAction(RegisterUserModel model)
        {
            this.model = model;
        }

        public RegisterUserModel model { get; }
    }
}
