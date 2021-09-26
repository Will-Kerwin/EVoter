using Microsoft.JSInterop;

namespace E_Voter.Services
{

    public interface ILocalStorageService
    {
        public Task<string> Get(string key);
        public Task Set(string key, string value);
        public Task Remove(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime JS;

        public LocalStorageService(IJSRuntime jS)
        {
            JS = jS;
        }

        public async Task<string> Get(string key)
        {
            return await JS.InvokeAsync<string>("localStorage.getItem", key);
        }

        public async Task Remove(string key)
        {
            await JS.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task Set(string key, string value)
        {
            await JS.InvokeVoidAsync("localStorage.setItem", key, value);
        }
    }
}
