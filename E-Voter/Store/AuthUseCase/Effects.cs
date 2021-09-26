using E_Voter.Models.Auth;
using E_Voter.Store.AuthUseCase.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;
using System.Net.Http.Json;

namespace E_Voter.Store.AuthUseCase
{
    public class Effects
    {
        private readonly HttpClient Http;
        private readonly ILogger<Effects> logger;
        private readonly string URI;
        private readonly NavigationManager NavMgr;
        private readonly ISnackbar Snackbar;

        public Effects(HttpClient http, ILogger<Effects> logger, NavigationManager navMgr)
        {
            Http = http;
            this.logger = logger;
            URI = "https://localhost:7284";
            NavMgr = navMgr;
        }

        [EffectMethod]
        public async Task HandleLoginUserData(LoginUserDataAction action, IDispatcher dispatcher)
        {
            try
            {
                var res = await Http.PostAsJsonAsync($"{URI}/Auth/Login", action.Credentials);
                if (!res.IsSuccessStatusCode)
                {
                    if (res.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Snackbar.Add("Incorrect email and password", Severity.Error);
                        dispatcher.Dispatch(new LoginUserDataActionResult(false, new UserModel()));
                    }
                    else
                    {
                        Snackbar.Add("Unknown error occured", Severity.Error);
                        dispatcher.Dispatch(new LoginUserDataActionResult(false, new UserModel()));
                    }
                }
                else
                {
                    UserModel user = await res.Content.ReadFromJsonAsync<UserModel>();

                    dispatcher.Dispatch(new LoginUserDataActionResult(true, user));
                    NavMgr.NavigateTo("/home");
                }
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}");
                throw;
            }
        }

        [EffectMethod]
        public async Task HandleRegisterUserData(RegisterUserDataAction action, IDispatcher dispatcher)
        {
            try
            {
                var res = await Http.PostAsJsonAsync($"{URI}/Auth/Register", action.model);
                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception("Error registering user");
                }
                dispatcher.Dispatch(new RegisterUserDataActionResult());
                NavMgr.NavigateTo("/login");
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}");
                throw;
            }
        }

    }
}
