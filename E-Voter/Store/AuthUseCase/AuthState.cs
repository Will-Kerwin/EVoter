using E_Voter.Models.Auth;

namespace E_Voter.Store.AuthUseCase
{
    public class AuthState
    {
        public AuthState(UserModel user, bool isLoading, bool isAuthenticated)
        {
            User = user;
            IsLoading = isLoading;
            IsAuthenticated = isAuthenticated;
        }

        public UserModel User { get; }

        public bool IsLoading { get; }

        public bool IsAuthenticated {  get; }

    }
}
