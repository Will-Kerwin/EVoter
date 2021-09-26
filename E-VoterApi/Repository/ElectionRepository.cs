using Dapper;
using E_VoterApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace E_VoterApi.Repository
{
    public class ElectionRepository : BaseRepository
    {
        public ElectionRepository() : base()
        {
        }

        public static async Task<bool> ElectionExists(Guid electionID)
        {

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var checkElection = await con.QuerySingleAsync<Guid?>(
                        $@"select electionID from Election where userID = '{electionID}'",
                        commandType: CommandType.Text
                        );
                return checkElection != null ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return false;
                throw;
            }
        }

        public static async Task<bool> NomineeExits(Guid userID)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var checkElection = await con.QuerySingleAsync<Guid?>(
                        $@"select userID from Nominee where userID = '{userID}'",
                        commandType: CommandType.Text
                        );
                return checkElection != null ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return false;
                throw;
            }
        }

        public static async Task<bool> HasNominees(Guid electionID)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var checkNominee = await con.QuerySingleAsync<Guid?>(
                        $@"select userID from Nominee where electionID = '{electionID}'",
                        commandType: CommandType.Text
                        );
                return checkNominee != null ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return false;
                throw;
            }
        }

        public async Task<(bool success, Guid electionID)> NewElection(ElectionModel election)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var result = await con.QuerySingleAsync<Guid>(
                    $@"insert into Election (electionID, electionStartDate, electionEndDate, electionName) 
                           output inserted.electionID
                           values (NEWID(), '{election.electionStartDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{election.electionEndDate.ToString("yyyy-MM-dd HH:mm:ss")}', N'{election.electionName}');",
                    commandType: CommandType.Text
                    );
                return (true, result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (false, new Guid());
                throw;
            }
        }

        public async Task<(int success, ElectionModel election)> GetElection(Guid electionID)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var result = await con.QuerySingleAsync<ElectionModel>(
                    $@"select * from Election where electionID = '{electionID}'",
                    commandType: CommandType.Text
                    );
                if (result == null)
                    return (0, new ElectionModel());
                else
                    return (1, result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (-1, new ElectionModel());
                throw;
            }
        }

        public async Task<(int success, List<ElectionModel> elections)> GetAllElections()
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var result = await con.QueryMultipleAsync(
                    $@"select * from CurrentElections",
                    commandType: CommandType.Text
                    );
                List<ElectionModel> elections = result.Read<ElectionModel>().ToList();
                if (elections.Count == 0)
                    return (0, new List<ElectionModel>());
                else
                    return (1, elections);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (-1, new List<ElectionModel>());
                throw;
            }
        }

        public async Task<(int status, string reason)> AddNominee(NomineeModel nominee)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);

                if (!(await AuthRepository.UserExists(nominee.userID)))
                    return (0, "User");

                if (!(await ElectionExists(nominee.electionID)))
                    return (0, "Election");

                if (!(await NomineeExits(nominee.userID)))
                    return (0, "Nominee");

                var result = await con.ExecuteAsync(
                    $@"insert into Nominee (electionID, userID, association, description)
                       values ('{nominee.electionID}', '{nominee.userID}', '{nominee.association}', N'{nominee.description}');",
                    commandType: CommandType.Text
                    );
                return (1, "");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.StackTrace}");
                return (-1,"");
                throw;
            }
        }

        public async Task<(int status, string reason)> RemoveNominee(Guid electionID, Guid userID)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);

                if (!(await AuthRepository.UserExists(userID)))
                    return (0, "User");

                if (!(await ElectionExists(electionID)))
                    return (0, "Election");

                var result = await con.ExecuteAsync(
                    $@"delete from Nominee where electionID = '{electionID}' and userID = '{userID}'",
                    commandType: CommandType.Text
                    );
                return (1, "");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (-1, "");
                throw;
            }
        }

        public async Task<(int status, List<TallyVoteModel> results, string reason)> TallyElection(Guid electionID)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);

                if (!(await ElectionExists(electionID)))
                    return (0, new List<TallyVoteModel>(), "Election");

                if (!(await HasNominees(electionID)))
                    return (0, new List<TallyVoteModel>(), "Nominee");

                var data = await con.QueryMultipleAsync(
                    $@"select n.userID as nominee, count(v.voteID) as votes from Vote as v 
                   join Nominee as n on v.electionID = n.electionID
                   where v.electionID = '{electionID}'
                   group by n.userID",
                    commandType: CommandType.Text
                    );

                var tally = await data.ReadAsync<TallyVoteModel>();

                return (1, tally.ToList(), "");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (-1, new List<TallyVoteModel>(), "");
                throw;
            }
        }

        public async Task<(int status, List<ElectionResultModel> electionResult , string reason)> SubmitResults(Guid electionID, List<TallyVoteModel> tally)
        {

            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);

                if (!(await ElectionExists(electionID)))
                    return (0, new List<ElectionResultModel>(), "Election");

                tally.Sort();

                int ranking = 1;
                foreach (TallyVoteModel vote in tally)
                {
                    await con.ExecuteAsync(
                        $@"insert into Results (nomineeID, electionID, totalVotes, ranking)
                       values ('{vote.nominee}', '{electionID}', '{vote.votes}', {ranking})",
                        commandType: CommandType.Text
                        );
                }

                var data = await con.QueryMultipleAsync(
                    $@"select * from Results where electionID = '{electionID}'",
                    commandType: CommandType.Text
                    );

                var results = await data.ReadAsync<ElectionResultModel>();

                return (1, results.ToList(), "");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (-1, new List<ElectionResultModel>(), "");
                throw;
            }
        }
    }
}
