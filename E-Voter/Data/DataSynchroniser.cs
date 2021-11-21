using E_Voter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Data.Common;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace E_Voter.Data
{
    public class DataSynchroniser
{
        public const string SqliteDbFilename = "app.db";
        private readonly Task firstTimeSetupTask;
        private readonly IDbContextFactory<ClientSideDbContext> dbContextFactory;
        private readonly ILogger<DataSynchroniser> logger;
        private bool isSynchronizing;

        public DataSynchroniser(IJSRuntime js, IDbContextFactory<ClientSideDbContext> dbContextFactory, ILogger<DataSynchroniser> logger)
        {
            this.dbContextFactory = dbContextFactory;
            firstTimeSetupTask = FirstTimeSetupAsync(js);
            this.logger = logger;
        }

        public int SyncCompleted { get; private set; }
        public int SyncTotal { get; private set; }

        public async Task<ClientSideDbContext> GetPreparedDbContextAsync()
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
            var module = await js.InvokeAsync<IJSObjectReference>("import", "./dbstorage.js");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
            {
                await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", SqliteDbFilename);
            }

            using var db = await dbContextFactory.CreateDbContextAsync();
            await db.Database.EnsureCreatedAsync();
        }

        record syncResponse (int modifiedCount, List<User> users);

        private async Task EnsureSynchronizingAsync()
        {
            if (isSynchronizing)
            {
                return;
            }

            try
            {
                isSynchronizing = true;
                SyncCompleted = 0;
                SyncTotal = 0;

                // Get a DB context
                using var db = await GetPreparedDbContextAsync();
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                // Begin fetching any updates to the dataset from the backend server
                var mostRecentUpdate = db.Users.OrderByDescending(p => p.modifiedTicks).FirstOrDefault()?.modifiedTicks;

                var connection = db.Database.GetDbConnection();
                connection.Open();

                while (true)
                {
                    HttpClient client = new HttpClient();
                    var request = await client.PostAsJsonAsync($"https://localhost:7026/Users", new { MaxCount = 5000, modifiedSinceTicks = mostRecentUpdate ?? -1 });
                    var response = await request.Content.ReadFromJsonAsync<syncResponse>();
                    var syncRemaining = response.modifiedCount - response.users.Count;
                    SyncCompleted += response.users.Count;
                    SyncTotal = SyncCompleted + syncRemaining;

                    if (response.users.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        mostRecentUpdate = response.users.Last().modifiedTicks;
                        BulkInsert(connection, response.users);
                        OnUpdate?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
                logger.LogError(ex.Message);
            }
            finally
            {
                isSynchronizing = false;
            }
        }

        private void BulkInsert(DbConnection connection, IEnumerable<User> users)
        {
            using (var transaction = connection.BeginTransaction())
            {
                var command = connection.CreateCommand();
                var userID = AddNamedParameter(command, "$userID");
                var email = AddNamedParameter(command, "$email");
                var firstName = AddNamedParameter(command, "$firstName");
                var lastName = AddNamedParameter(command, "$lastName");
                var contactNo = AddNamedParameter(command, "$contactNo");
                var modifiedTicks = AddNamedParameter(command, "$modifiedTicks");

                command.CommandText =
                    $"INSERT OR REPLACE INTO VoterUsers (userId, email, firstName, lastName, contactNo, modifiedTicks) " +
                    $"VALUES ({userID.ParameterName}, {email.ParameterName}, {firstName.ParameterName}, {lastName.ParameterName}, {contactNo.ParameterName}, {modifiedTicks.ParameterName})";

                foreach (var user in users)
                {
                    userID.Value = user.userID;
                    email.Value = user.email;
                    firstName.Value = user.firstName;
                    lastName.Value = user.lastName;
                    contactNo.Value = user.contactNo;
                    modifiedTicks.Value = user.contactNo;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        static DbParameter AddNamedParameter(DbCommand command, string name)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return parameter;
        }
    }
}
