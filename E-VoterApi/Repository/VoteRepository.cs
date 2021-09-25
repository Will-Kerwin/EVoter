using Dapper;
using E_VoterApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace E_VoterApi.Repository
{
    public class VoteRepository : BaseRepository
    {
        public VoteRepository() : base()
        {
        }

        public async Task<(int status, string reason)> SubmitVote(VoteModel vote)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);

                if (!(await ElectionRepository.NomineeExits(vote.nomineeID)))
                    return (0, "Nominee");

                if (!(await AuthRepository.UserExists(vote.voterID)))
                    return (0, "Voter");

                if (!(await ElectionRepository.ElectionExists(vote.electionID)))
                    return (0, "Election");

                var result = await con.ExecuteAsync(
                    $@"insert into Vote (voteID, nomineeID, electionID, voterID)
                   values (NEWID(), {vote.nomineeID}, {vote.electionID}, {vote.voterID})",
                    commandType: CommandType.Text
                    );
                return (1, "");
            }
            catch (Exception e)
            {
                return (-1, "");
                throw;
            }

        }

        public async Task<(int status, int total,string reason)> GetVotesForNominee(Guid nomineeID, Guid electionID)
        {
            using SqlConnection con = new SqlConnection(ConnectionString);

            if (!(await ElectionRepository.NomineeExits(nomineeID)))
                return (0, 0, "Nominee");

            if (!(await ElectionRepository.ElectionExists(electionID)))
                return (0, 0, "Election");

            var result = await con.QuerySingleAsync<int>(
                    $@"select count(voteID) as total from Vote where nomineeID = {nomineeID} and electionID = {electionID};",
                    commandType: CommandType.Text
                    );
            return (1, result , "");

        }
    }
}
