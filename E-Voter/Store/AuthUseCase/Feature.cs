using Fluxor;

namespace E_Voter.Store.AuthUseCase
{
    public class Feature : Feature<AuthState>
    {

        public override string GetName() => "Auth";
        protected override AuthState GetInitialState() => new AuthState(
                user: new Models.Auth.UserModel(),
                isLoading: false,
                isAuthenticated: false
            );

    }
}
