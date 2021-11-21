using E_Voter;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Fluxor;
using E_Voter.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

builder.Services.AddMudServices();

builder.Services.AddFluxor(o =>
{
    o.UseRouting();
    o.UseReduxDevTools();
    o.ScanAssemblies(typeof(Program).Assembly);
});

builder.Services.AddElectionDataDbContext();

await builder.Build().RunAsync();
