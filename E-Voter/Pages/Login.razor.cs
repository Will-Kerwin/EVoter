using E_Voter.Models.Auth;
using E_Voter.Store.AuthUseCase;
using E_Voter.Store.AuthUseCase.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace E_Voter.Pages
{
    public partial class Login
    {

        MudForm form;
        bool success;
        string[] errors;
        LoginDetailsModel model = new LoginDetailsModel();

        [Inject]
        public IDispatcher dispatcher { get; set; }

        [Inject]
        public IState<AuthState> AuthState { get; set; }

        [Inject]
        public NavigationManager NavMgr { get; set; }

        private void LoginUser()
        {
            form.Validate();
            if (form.IsValid)
            {
                dispatcher.Dispatch(new LoginUserDataAction(model));
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (AuthState.Value.IsAuthenticated)
            {
                NavMgr.NavigateTo("/home");
            }
        }
    }
}
