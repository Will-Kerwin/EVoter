using E_Voter.Store.AuthUseCase.Actions;
using Fluxor;

namespace E_Voter.Store.AuthUseCase
{
    public static class Reducers
    {

        [ReducerMethod]
        public static AuthState ReduceRegisterUserDataAction(AuthState state, RegisterUserDataAction action) =>
            new AuthState(
                    isLoading: true,
                    user: state.User,
                    isAuthenticated: state.IsAuthenticated
                );

        [ReducerMethod]
        public static AuthState ReduceRegisterUserDataActionResult(AuthState state, RegisterUserDataActionResult action) =>
            new AuthState(
                    isLoading: false,
                    user: state.User,
                    isAuthenticated: state.IsAuthenticated
                );

        [ReducerMethod]
        public static AuthState ReduceLoginUserDataAction(AuthState state, LoginUserDataAction action) =>
            new AuthState(
                    isLoading: true,
                    user: state.User,
                    isAuthenticated: state.IsAuthenticated
                );

        [ReducerMethod]
        public static AuthState ReduceLoginUserDataActionResult(AuthState state, LoginUserDataActionResult action) =>
            new AuthState(
                    isLoading: false,
                    user: action.User,
                    isAuthenticated: action.IsAuthenticated
                );
    }
}
