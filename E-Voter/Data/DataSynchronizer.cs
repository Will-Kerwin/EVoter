using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace E_Voter.Data
{
    class DataSynchronizer
    {
        public const string SqliteDbFilename = "app.db";
        private readonly Task firstTimeSetupTask;
        private readonly IDbContextFactory<ClientSideDBContext> dbContextFactory;
        private bool isSyncronizing;

        public DataSynchronizer(IJSRuntime js, IDbContextFactory<ClientSideDBContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            firstTimeSetupTask = FirstTimeSetupAsync(js);
        }

        public int SyncCompleted { get; set; }
        public int SyncTotal { get; private set; }

        public async Task<ClientSideDBContext> GetPreparedDbContextAsync()
        {
            await firstTimeSetupTask;
            return await dbContextFactory.CreateDbContextAsync();
        }

        public void SynchronizeInBackground()
        {
            _ = EnsureSynchronizingAsync();
        }

        public event Action? OnUpdate;
        public event Action<Exception>? OnError;

        private async Task FirstTimeSetupAsync(IJSRuntime js)
        {
            // need to find function of this js file
            var module = await js.InvokeAsync<IJSObjectReference>("import", "./dbstorage.js");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
                await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", SqliteDbFilename);

            using var db = await dbContextFactory.CreateDbContextAsync();
            await db.Database.EnsureCreatedAsync();
        }

        private async Task EnsureSynchronizingAsync()
        {
            // Don't run multiple syncs in parallel. This simple logic is adequate because of single-threadedness.
            if (isSyncronizing)
            {
                return;
            }

            try
            {
                isSyncronizing = true;
                SyncCompleted = 0;
                SyncTotal = 0;

                //get db context
                using var db = await GetPreparedDbContextAsync();
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                // begin fetch any updates to dataset from backend server
                var mostRecentUpdate = db.Elections.OrderByDescending(p => p.electionID).FirstOrDefault()?.electionID;

                var connection = db.Database.GetDbConnection();
                connection.Open();

                while (true)
                {
                    HttpClient client = new HttpClient();
                    var res = client.GetAsync("https://localhost:7284/Elections");
                }
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
            }
            finally
            {
                isSyncronizing = false;
            }
        }
    }
}
