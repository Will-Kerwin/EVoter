using E_Voter.Models.Auth;
using E_Voter.Store.AuthUseCase;
using E_Voter.Store.AuthUseCase.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace E_Voter.Pages
{
    public partial class Register
    {

        MudForm form;
        bool success;
        string[] errors;
        RegisterUserModel model = new RegisterUserModel();

        [Inject]
        public IDispatcher dispatcher { get; set; }

        [Inject]
        public IState<AuthState> AuthState { get; set;  }

        [Inject]
        public NavigationManager NavMgr { get; set; }

        private string ValidPhone(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                return "Telephone is required!";
            if (arg.Length < 11)
                return "Telephone is invalid";
            if (!Regex.IsMatch(arg, @"^[0-9]*$"))
                return "Telephone is invalid";
            return null;
        }

        private void RegisterUser()
        {
            form.Validate();
            if (form.IsValid)
            {
                dispatcher.Dispatch(new RegisterUserDataAction(model));
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
