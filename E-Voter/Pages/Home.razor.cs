using E_Voter.Store.AuthUseCase;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace E_Voter.Pages
{
    public partial class Home
    {

        [Inject]
        public IState<AuthState> AuthState { get; set; }

    }
}
